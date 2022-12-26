<script setup lang="ts">
import UserMenu from "@/components/user/UserMenu.vue";
import Header from "@/components/common/Header.vue";
import { Right, Help, GobletSquare } from "@element-plus/icons-vue";
import { useUserCourseLearnStore } from "@/store/user/userCourseLearnStore";
import { storeToRefs } from "pinia";
import { onMounted, reactive, ref } from "vue";
import { ElMessage, FormInstance } from "element-plus";
import { queryClient } from "@/helpers/queryClient";
import {
  ISupportCourseDataIssueModel,
  ILearnCourseGetModel,
  ILearnCourseModel,
  ILearnCourseSendModel,
  ILearnCourseCheckResponseModel,
  ILearnCourseCheckModel,
  ILearnCourseNextResponseModel,
  ILearnCourseNextModel,
} from "@/services/user/types";
import {
  sendSupportIssue,
  getLearnData,
  learnCourseCheck,
  getNextData,
} from "@/services/user/userApi";
import { ISupportIssueTypeOptionModel } from "@/services/admin/types";
import { getSupportIssuesTypesOptionsList } from "@/services/admin/adminApi";
import { IUserAuth } from "@/services/auth/types";
import { useRoute } from "vue-router";

const store = useUserCourseLearnStore();
const route = useRoute();
const auth = queryClient.getQueryData(["authUser"]) as IUserAuth;
const { currentCourse } = storeToRefs(store);
const checkResponse = ref<ILearnCourseCheckResponseModel | null>();
const checkSent = ref(false);
const supportDialog = ref<boolean>(false);
const isSending = ref<boolean>(false);
const issueTypesOptions = ref<ISupportIssueTypeOptionModel[] | null>();
const formRef = ref<FormInstance>();
const expendedExamples = ref("1");

const learnCourseSendModel: ILearnCourseSendModel = reactive({
  userCoursePublicId: "",
  source: "",
  translation: "",
});

const learnCourseGetModel: ILearnCourseGetModel = reactive({
  userId: "",
  coursePublicId: "",
});
const supportIssueModel: ISupportCourseDataIssueModel = reactive({
  typePublicId: "",
  reporterId: "",
  issueMessage: "",
  issueEntityId: "",
});

onMounted(() => {
  if (route.params.id !== undefined) {
    learnCourseGetModel.coursePublicId = route.params.id.toString();
    learnCourseGetModel.userId = auth.id;
    getLearnCourseFn(learnCourseGetModel);
    getIssueTypesOptions();
  }
});

const supportIssueRules = reactive({
  issueMessage: [{ required: true, message: "Message is required" }],
});

const getLearnCourseFn = (model: ILearnCourseGetModel) => {
  getLearnData(model)
    .then((data: ILearnCourseModel) => {
      currentCourse.value = data;
      learnCourseSendModel.userCoursePublicId = data.userCoursePublicId;
      learnCourseSendModel.source = data.source;
    })
    .catch((error: any) => {
      error.response.data.Errors.forEach(async (e: any) => {
        ElMessage({
          showClose: true,
          message: e.Title,
          type: "error",
        });
      });
    });
};

const sendLearnCourseCheck = (model: ILearnCourseCheckModel) => {
  learnCourseCheck(model)
    .then((data: ILearnCourseCheckResponseModel) => {
      if (!data.isError) {
        ElMessage({
          showClose: true,
          message: "Correct!",
          type: "success",
        });
      } else {
        ElMessage({
          showClose: true,
          message: "Incorrect translation!",
          type: "warning",
        });
      }
      checkResponse.value = data;
      currentCourse.value!.percentProgress = data.percentProgress;
      checkSent.value = true;
    })
    .catch((error: any) => {
      error.response.data.Errors.forEach(async (e: any) => {
        ElMessage({
          showClose: true,
          message: e.Title,
          type: "error",
        });
      });
    });
};

const getNextLearnCourseData = (model: ILearnCourseNextModel) => {
  getNextData(model)
    .then((data: ILearnCourseNextResponseModel) => {
      currentCourse.value!.currentDataPublicId = data.currentDataPublicId;
      learnCourseSendModel.source = data.source;
      checkResponse.value = null;
      checkSent.value = false;
      learnCourseSendModel.translation = "";
    })
    .catch((error: any) => {
      error.response.data.Errors.forEach(async (e: any) => {
        ElMessage({
          showClose: true,
          message: e.Title,
          type: "error",
        });
      });
    });
};

const sendIssue = (model: ISupportCourseDataIssueModel) => {
  sendSupportIssue(model)
    .then((data) => {
      ElMessage({
        showClose: true,
        message: "Issue has been sent",
        type: "success",
      });
      supportDialog.value = false;
      isSending.value = false;
    })
    .catch((error: any) => {
      error.response.data.Errors.forEach(async (e: any) => {
        ElMessage({
          showClose: true,
          message: e.Title,
          type: "error",
        });
      });
    });
};

const getIssueTypesOptions = async () => {
  await getSupportIssuesTypesOptionsList()
    .then((data: ISupportIssueTypeOptionModel[]) => {
      let options: ISupportIssueTypeOptionModel[] = [];
      data.forEach((el) => {
        options.push(el);
      });
      issueTypesOptions.value = options;
    })
    .catch((error) => {
      error.response.data.Errors.forEach(async (e: any) => {
        ElMessage({
          showClose: true,
          message: e.Title,
          type: "error",
        });
      });
    });
};

const submitSupportIssue = async (formEl: FormInstance | undefined) => {
  if (!formEl) return;
  await formEl.validate((valid, fields) => {
    if (valid) {
      isSending.value = true;
      const typePublicId = issueTypesOptions.value?.find(
        (type) => type.name == "CourseDataIssue"
      )?.publicId;
      sendIssue({
        issueMessage: supportIssueModel.issueMessage,
        issueEntityId:
          currentCourse.value?.currentDataPublicId != undefined
            ? currentCourse.value?.currentDataPublicId
            : "",
        reporterId: auth.id,
        typePublicId: typePublicId != undefined ? typePublicId : "",
      });
    } else {
      console.log("error submit!", fields);
    }
  });
};

const submitCheck = async () => {
  sendLearnCourseCheck({
    userId: auth.id,
    userCoursePublicId: learnCourseSendModel.userCoursePublicId,
    translation: learnCourseSendModel.translation,
  });
};

const submitNext = async () => {
  getNextLearnCourseData({
    userId: auth.id,
    userCoursePublicId: learnCourseSendModel.userCoursePublicId,
  });
};
</script>

<template>
  <Header></Header>
  <div class="flex grow">
    <UserMenu />
    <div class="grow flex flex-col p-2">
      <el-card class="box-card">
        <template #header>
          <div class="grow grid grid-cols-3 gap-2">
            <div class="font-bold">
              <span>{{
                currentCourse?.languageFrom + " - " + currentCourse?.languageTo
              }}</span>
            </div>
            <div class="text-center">
              <span class="font-bold text-xl">{{ currentCourse?.name }}</span>
            </div>
            <div class="text-bold text-black">
              <el-progress
                :text-inside="true"
                :stroke-width="24"
                :percentage="currentCourse?.percentProgress"
                status="success"
              />
            </div>
          </div>
        </template>
        <div class="flex justify-center">
          <div
            v-if="checkResponse?.isFinished || currentCourse?.isFinished"
            class="2xl:w-3/5 xl:w-4/5 lg:w-full md:w-full sm:w-full justify-center items-center m-16"
          >
            <div
              class="flex font-bold text-4xl text-yellow-400 justify-center items-center"
            >
              <el-icon><GobletSquare /></el-icon>
              <span>Congratulations!</span>
              <el-icon><GobletSquare /></el-icon>
            </div>
            <div class="flex font-bold text-xl justify-center items-center">
              <span>You've finished this course!</span>
            </div>
            <div class="flex justify-center items-center mt-5">
              <el-button @click="$router.push({ name: 'user-dashboard' })">
                Learn more here!
              </el-button>
            </div>
          </div>
          <div
            v-else
            class="2xl:w-3/5 xl:w-4/5 lg:w-full md:w-full sm:w-full bg-slate-100"
          >
            <div class="m-5 p-5">
              <div class="flex">
                <div class="flex flex-row">
                  <span class="font-bold text-xl">Translate</span>
                </div>
                <div class="flex flex-row !ml-auto">
                  <el-button
                    class="min-w-full"
                    type="danger"
                    plain
                    @click="supportDialog = true"
                  >
                    <el-icon class="text-xl"><Help /></el-icon>
                    <span>Error</span>
                  </el-button>
                </div>
              </div>
              <div class="mt-5 grow grid grid-cols-11 gap-2">
                <div class="col-span-5 text-center">
                  <span>{{ currentCourse?.languageFrom }}</span>
                  <el-input
                    v-model="learnCourseSendModel.source"
                    readonly
                    class="col-span-5"
                    clearable
                    size="large"
                    :placeholder="'Translation from '"
                  />
                </div>
                <div
                  class="flex items-center justify-center text-center text-3xl"
                >
                  <el-icon><Right /></el-icon>
                </div>
                <div class="col-span-5 text-center">
                  <span>{{ currentCourse?.languageTo }}</span>
                  <el-input
                    v-model="learnCourseSendModel.translation"
                    :disabled="checkSent"
                    clearable
                    autofocus
                    size="large"
                    :placeholder="'Translation to '"
                  />
                </div>
              </div>
              <el-collapse
                class="mt-5"
                v-if="checkResponse?.hasExamples"
                v-model="expendedExamples"
                accordion
              >
                <el-collapse-item
                  title="&nbsp;&nbsp;&nbsp;&nbsp;Example sentenses"
                  name="1"
                  class="border-2"
                >
                  <el-table
                    :data="checkResponse?.examples"
                    stripe
                    style="width: 100%"
                  >
                    <el-table-column
                      prop="source"
                      :label="currentCourse?.languageFrom"
                    />
                    <el-table-column
                      prop="translation"
                      :label="currentCourse?.languageTo"
                    />
                  </el-table>
                </el-collapse-item>
              </el-collapse>
              <div
                v-if="checkResponse?.isError"
                class="mt-5 p-2 text-center border-2 border-red-500 font-bold"
              >
                <span
                  >Incorrect translation:
                  {{
                    learnCourseSendModel.translation.slice(
                      0,
                      checkResponse.wordIndexError
                    )
                  }}<span class="text-red-500">{{
                    learnCourseSendModel.translation.slice(
                      checkResponse.wordIndexError
                    )
                  }}</span></span
                >
              </div>
              <div class="mt-5 flex ml-auto">
                <el-button
                  v-if="checkSent"
                  class="min-w-full bg-white"
                  plain
                  type="info"
                  @click="submitNext"
                  >Next</el-button
                >
                <el-button
                  v-else
                  class="min-w-full bg-white"
                  type="success"
                  plain
                  @click="submitCheck"
                  >Check</el-button
                >
              </div>
            </div>
          </div>
        </div>
      </el-card>
    </div>
  </div>

  <el-dialog
    v-model="supportDialog"
    title="Report a bug with course data"
    width="40%"
    center
  >
    <el-form
      class="my-2"
      label-position="left"
      label-width="auto"
      ref="formRef"
      :model="supportIssueModel"
      :rules="supportIssueRules"
    >
      <span>Send issue with data to system administrators</span>
      <el-form-item class="text-lg mt-2" prop="issueMessage" label="Message">
        <el-input
          v-model="supportIssueModel.issueMessage"
          type="textarea"
          placeholder="Describe an issue/issues"
          size="large"
        />
      </el-form-item>
    </el-form>
    <template #footer>
      <span class="dialog-footer">
        <el-button plain @click="supportDialog = false">Cancel</el-button>
        <el-button
          type="success"
          :loading="isSending"
          plain
          @click="submitSupportIssue(formRef)"
          >Send</el-button
        >
      </span>
    </template>
  </el-dialog>
</template>

<style scoped></style>
