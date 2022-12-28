<script setup lang="ts">
import UserMenu from "@/components/user/UserMenu.vue";
import { RefreshRight, Search } from "@element-plus/icons-vue";
import Header from "@/components/common/Header.vue";
import { getLanguageOptionsList } from "@/services/admin/adminApi";
import {
  getAvailableCourseList,
  addUserToCourse,
  getActiveCourseList,
} from "@/services/user/userApi";
import { ElMessage } from "element-plus";
import { storeToRefs } from "pinia";
import { onMounted, ref } from "vue";
import {
  ICourseActiveListModel,
  ICourseListModel,
  ILanguageModel,
} from "@/services/admin/types";
import { useUserCourseStore } from "@/store/user/userCourseStore";
import { IPagedResult, IPager } from "@/services/common/types";
import { getCurrUser } from "@/services/auth/authApi";
import { useQuery } from "@tanstack/vue-query";
import { IAddUserToCourseModel } from "@/services/user/types";

const store = useUserCourseStore();
const userId = ref<string>("");
const { filter, isSearching, descriptionLength } = storeToRefs(store);
const languageOptions = ref<ILanguageModel[] | null>();
const focusedItem = ref<ICourseActiveListModel>();
let entities = ref<ICourseActiveListModel[]>();
const auth = useQuery(["authUser"], getCurrUser);
const courseDescriptionDialog = ref(false);
const courseDescriptionDialogTitle = ref("");

const getLanguageOptions = async () => {
  await getLanguageOptionsList()
    .then((data: ILanguageModel[]) => {
      let options: ILanguageModel[] = [];
      data.forEach((el) => {
        options.push(el);
      });
      languageOptions.value = options;
    })
    .catch((error) => {
      isSearching.value = false;
      error.response.data.Errors.forEach(async (e: any) => {
        ElMessage({
          showClose: true,
          message: e.Title,
          type: "error",
        });
      });
    });
};

onMounted(async () => {
  userId.value = auth.data.value?.id != undefined ? auth.data.value?.id : "";
  if (userId.value != "") {
    await getEntities();
  }
  getLanguageOptions();
});

const getEntities = () => {
  isSearching.value = true;
  getActiveCourseList(userId.value, filter.value)
    .then((data: any) => {
      isSearching.value = false;
      entities.value = data;
    })
    .catch((error) => {
      isSearching.value = false;
      error.response.data.Errors.forEach(async (e: any) => {
        ElMessage({
          showClose: true,
          message: e.Title,
          type: "error",
        });
      });
    });
};

const cutDescription = (desc: string) => {
  return desc.length > descriptionLength.value
    ? desc.substring(0, descriptionLength.value - 3) + "..."
    : desc;
};

const resetFilters = () => {
  store.resetFilters();
  getEntities();
};

const handleDescriptionDialog = (course: ICourseActiveListModel) => {
  courseDescriptionDialog.value = true;
  courseDescriptionDialogTitle.value = `Course: ${course.name}`;
  focusedItem.value = course;
};
</script>

<template>
  <Header></Header>
  <div class="flex grow">
    <UserMenu />
    <div class="grow flex flex-col p-2">
      <div class="flex">
        <span class="font-bold text-xl">Active courses</span>
      </div>
      <div>
        <div class="flex mt-2">
          <div class="flex flex-row">
            <el-input
              v-model="filter.name"
              class="mb-2"
              placeholder="Course name"
            />
            <el-select
              v-model="filter.languageFrom"
              multiple
              collapse-tags
              collapse-tags-tooltip
              placeholder="Language from"
              clearable
              @clear="getEntities"
              class="ml-2 min-w-fit"
            >
              <el-option
                v-for="item in languageOptions"
                :key="item.id"
                :label="item.name"
                :value="item.id"
              />
            </el-select>
            <el-select
              v-model="filter.languageTo"
              multiple
              collapse-tags
              collapse-tags-tooltip
              placeholder="Language to"
              clearable
              @clear="getEntities"
              class="ml-2 min-w-fit"
            >
              <el-option
                v-for="item in languageOptions"
                :key="item.id"
                :label="item.name"
                :value="item.id"
              />
            </el-select>
          </div>
          <div class="flex flex-row !ml-auto">
            <el-button
              class="mb-2 mr-2 p-3 font-bold right !ml-auto"
              plain
              :loading="isSearching"
              @click="getEntities"
              ><el-icon><Search /></el-icon>&nbsp;Search</el-button
            >
            <el-button
              class="mb-2 mr-2 p-3 font-bold right !ml-auto"
              plain
              type="info"
              @click="resetFilters"
              ><el-icon><RefreshRight /></el-icon>&nbsp;Reset filters</el-button
            >
          </div>
        </div>
        <div
          v-if="entities !== undefined && entities.length > 0"
          class="grow grid lg:grid-cols-2 md:grid-cols-1 grid-rows-2 gap-2"
        >
          <el-card
            v-for="course in entities"
            shadow="hover"
            class="box-card"
            body-style="display: flex; flex-flow:column; height: 85%;"
          >
            <template #header>
              <div class="card-header text-center">
                <span class="font-bold">{{ course.name }}</span>
              </div>
            </template>
            <div class="grid grid-cols-2 h-full">
              <div class="mr-2">
                <div class="font-bold">
                  <span>Languages:</span>
                </div>
                <span>{{ course.languageFrom }} - {{ course.languageTo }}</span>
                <el-divider
                  style="margin: 8px 8px 0 -10px !important"
                  class="col-span-2"
                  direction="horizontal"
                />
                <div
                  v-if="course.description !== ''"
                  class="cursor-pointer"
                  @click="handleDescriptionDialog(course)"
                >
                  <div class="font-bold">
                    <span>Description:</span>
                  </div>
                  <span>{{ cutDescription(course.description) }}</span>
                </div>
              </div>
              <div class="grid grid-col-2 gap-2 text-center items-center mb-5">
                <div
                  class="flex h-full border-2 bg-slate-100 border-neutral-300 items-center justify-center"
                >
                  <div>
                    <div>
                      <span>Completed</span>
                    </div>
                    <div>
                      <span class="font-bold text-4xl h-full"
                        >{{ course.percentProgress }}%</span
                      >
                    </div>
                  </div>
                </div>
                <div
                  class="flex h-full border-2 bg-slate-100 border-rose-500 items-center justify-center"
                >
                  <div>
                    <div>
                      <span>Amount of errors</span>
                    </div>
                    <span class="font-bold text-red-600 text-4xl">{{
                      course.amountOfErrors
                    }}</span>
                  </div>
                </div>
                <div class="col-span-2 h-full bg-slate-100">
                  <el-button
                    class="min-w-full min-h-full text-xl"
                    type="success"
                    plain
                    @click="
                      $router.push({
                        name: 'user-learn',
                        params: { id: course.id },
                      })
                    "
                  >
                    Learn
                  </el-button>
                </div>
              </div>
            </div>
          </el-card>
        </div>
        <div v-else>
          <el-card
            shadow="never"
            class="box-card mt-2"
            body-style="display: flex; flex-flow:column; height: 85%;"
          >
            <div class="text-center text-2xl">You didn't join any courses.</div>
            <div class="text-center text-xl">
              You can search available courses
              <router-link
                class="text-blue-500 underline"
                :to="{ name: 'user-course-list' }"
                >here</router-link
              >
            </div>
          </el-card>
        </div>
      </div>
    </div>
  </div>

  <el-dialog
    v-model="courseDescriptionDialog"
    :title="courseDescriptionDialogTitle"
    width="70%"
    center
  >
    <el-descriptions :column="1" :border="true" class="mt-2">
      <el-descriptions-item label-class-name="w-32" label="Course name:">{{
        focusedItem?.name
      }}</el-descriptions-item>
      <el-descriptions-item label-class-name="w-32" label="Languages: ">{{
        focusedItem?.languageFrom + " - " + focusedItem?.languageTo
      }}</el-descriptions-item>
      <el-descriptions-item label-class-name="w-32" label="Description: ">
        <p v-for="text in focusedItem?.description.split('\n')">
          {{ text }}
        </p>
      </el-descriptions-item>
    </el-descriptions>
    <template #footer>
      <span class="dialog-footer">
        <el-button plain @click="courseDescriptionDialog = false"
          >Cancel</el-button
        >
      </span>
    </template>
  </el-dialog>
</template>

<style scoped></style>
