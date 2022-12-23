<script setup lang="ts">
import { Help, House, Collection } from "@element-plus/icons-vue";
import { useAuthMenuStore } from "@/store/common/authMenuStore";
import { storeToRefs } from "pinia";
import { router } from "@/router";
import { queryClient } from "@/helpers/queryClient";
import { onMounted, reactive, ref } from "vue";
import { ElMessage, FormInstance } from "element-plus";
import { ISupportApplicationIssueModel } from "@/services/user/types";
import { sendSupportApplicationIssue } from "@/services/user/userApi";
import { IUserAuth } from "@/services/auth/types";
import { getSupportIssuesTypesOptionsList } from "@/services/admin/adminApi";
import { ISupportIssueTypeOptionModel } from "@/services/admin/types";

const authMenuStore = useAuthMenuStore();
const { isMenuCollapsed } = storeToRefs(authMenuStore);
const supportDialog = ref<boolean>(false);
const issueTypesOptions = ref<ISupportIssueTypeOptionModel[] | null>();
const auth = queryClient.getQueryData(["authUser"]) as IUserAuth;

const formRef = ref<FormInstance>();
const supportApplicationIssueModel: ISupportApplicationIssueModel = reactive({
  typePublicId: "",
  reporterId: "",
  issueMessage: "",
});

onMounted(() => {
  getIssueTypesOptions();
});

const supportApplicationIssueRules = reactive({
  issueMessage: [{ required: true, message: "Message is required" }],
});

const sendIssue = (model: ISupportApplicationIssueModel) => {
  sendSupportApplicationIssue(model)
    .then(() => {
      ElMessage({
        showClose: true,
        message: "Issue has been sent",
        type: "success",
      });
      supportDialog.value = false;
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
      const typePublicId = issueTypesOptions.value?.find(
        (type) => type.name == "ApplicationIssue"
      )?.publicId;
      sendIssue({
        issueMessage: supportApplicationIssueModel.issueMessage,
        reporterId: auth.id,
        typePublicId: typePublicId != undefined ? typePublicId : "",
      });
    } else {
      console.log("error submit!", fields);
    }
  });
};
</script>

<template>
  <el-menu
    class="el-menu-vertical shadow-md flex flex-col"
    :collapse="isMenuCollapsed"
    :collapse-transition="false"
    :router="true"
    active-text-color="black"
    :default-active="$route.path"
  >
    <el-menu-item
      :index="
        router.getRoutes().filter((x) => x.name == 'user-dashboard')[0].path
      "
      :route="{ name: 'user-dashboard' }"
    >
      <el-icon><House /></el-icon>
      <span>Dashboard</span>
    </el-menu-item>
    <el-menu-item
      :index="
        router.getRoutes().filter((x) => x.name == 'user-course-list')[0].path
      "
      :route="{ name: 'user-course-list' }"
    >
      <el-icon><Collection /></el-icon>
      <span>Available courses</span>
    </el-menu-item>
    <el-menu-item class="mt-auto" index="" @click="supportDialog = true">
      <el-icon><Help /></el-icon>
      <span>Support</span>
    </el-menu-item>
  </el-menu>

  <el-dialog
    v-model="supportDialog"
    title="Support - report a bug"
    width="30%"
    center
  >
    <el-form
      class="my-2"
      label-position="left"
      label-width="auto"
      ref="formRef"
      :model="supportApplicationIssueModel"
      :rules="supportApplicationIssueRules"
    >
      <span class="font-bold text-lg"
        >Send application issue to system administrators</span
      >
      <el-form-item class="text-lg mt-2" prop="issueMessage" label="Message">
        <el-input
          v-model="supportApplicationIssueModel.issueMessage"
          type="textarea"
          placeholder="Describe an issue/issues with application"
          size="large"
        />
      </el-form-item>
    </el-form>
    <template #footer>
      <span class="dialog-footer">
        <el-button plain @click="supportDialog = false">Cancel</el-button>
        <el-button type="success" plain @click="submitSupportIssue(formRef)"
          >Send</el-button
        >
      </span>
    </template>
  </el-dialog>
</template>

<style scoped>
.is-active > .el-sub-menu__title > span,
.el-menu-item.is-active {
  font-weight: bold;
}
.el-menu-vertical:not(.el-menu--collapse) {
  min-width: 200px;
  min-height: 400px;
}
</style>
