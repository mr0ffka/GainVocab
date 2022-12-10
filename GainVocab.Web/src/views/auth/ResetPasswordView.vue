<script setup lang="ts">
import { onMounted, reactive, ref } from "vue";
import { resetPasswordFn } from "@/services/auth/authApi";
import type { IResetPasswordModel } from "@/services/auth/types";
import { ElMessage, FormInstance } from "element-plus";
import { useQueryClient } from "@tanstack/vue-query";
import router from "@/router";
import { useRoute, useRouter } from "vue-router";

const formRef = ref<FormInstance>();
const route = useRoute();

const forgotPasswordModel: IResetPasswordModel = reactive({
  userId: "",
  resetToken: "",
  newPassword: "",
  newPasswordConfirm: "",
});

const rules = reactive({
  newPassword: [{ required: true, message: "Password is required" }],
  newPasswordConfirm: [
    { required: true, message: "Password confirmation is required" },
  ],
});

onMounted(async () => {
  forgotPasswordModel.userId =
    typeof route.query.userid === "string"
      ? route.query.userid
      : route.query.userid?.[0] ?? "";
  forgotPasswordModel.resetToken =
    typeof route.query.token === "string"
      ? route.query.token
      : route.query.token?.[0] ?? "";

  if (
    forgotPasswordModel.userId == "" ||
    !forgotPasswordModel.userId ||
    forgotPasswordModel.resetToken == "" ||
    !forgotPasswordModel.resetToken
  ) {
    ElMessage({
      showClose: true,
      message: "Reset password link is invalid!",
      type: "error",
    });
  }
});

const resetPassword = (form: IResetPasswordModel) =>
  resetPasswordFn(form)
    .then(() => {
      ElMessage({
        showClose: true,
        message: "Something went wrong",
        type: "error",
      });
    })
    .catch(() => {
      ElMessage({
        showClose: true,
        message: "Password changed successfully!",
        type: "success",
      });
      router.push({ name: "login" });
    });

const submitForm = async (formEl: FormInstance | undefined) => {
  if (!formEl) return;
  await formEl.validate((valid, fields) => {
    if (valid) {
      resetPassword({
        userId: forgotPasswordModel.userId,
        resetToken: forgotPasswordModel.resetToken,
        newPassword: forgotPasswordModel.newPassword,
        newPasswordConfirm: forgotPasswordModel.newPasswordConfirm,
      });
    } else {
      console.log("error submit!", fields);
    }
  });
};
</script>

<template>
  <main class="flex justify-center items-center h-screen drop-shadow">
    <div class="border-grey border p-10 w-2/5 shadow-md">
      <div class="font-bold text-center mb-5 text-lg">Reset password</div>
      <el-form
        label-position="top"
        ref="formRef"
        :model="forgotPasswordModel"
        class="text-right"
        :rules="rules"
      >
        <el-form-item class="text-lg" prop="password">
          <label>New password</label>
          <el-input
            v-model="forgotPasswordModel.newPassword"
            type="password"
            placeholder="Password"
            show-password
            size="large"
          />
        </el-form-item>
        <el-form-item class="text-lg" prop="passwordConfirm">
          <label>New password confirmation</label>
          <el-input
            v-model="forgotPasswordModel.newPasswordConfirm"
            type="password"
            placeholder="Password"
            show-password
            size="large"
          />
        </el-form-item>
        <el-button size="large" @click="submitForm(formRef)" class=""
          >Reset password</el-button
        >
      </el-form>
    </div>
  </main>
</template>
