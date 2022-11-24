<script setup lang="ts">
import { onBeforeUpdate, onMounted, reactive, ref } from "vue";
import {
  loginUserFn,
  registerUserFn,
  resetPasswordFn,
} from "@/services/authApi";
import type {
  GenericResponse,
  IResetPasswordModel,
  IRegisterModel,
} from "@/services/types";
import { ElMessage, FormInstance } from "element-plus";
import { useMutation, useQuery, useQueryClient } from "@tanstack/vue-query";
import router from "@/router";
import { IUserResponse } from "@/services/types";
import { useRoute, useRouter } from "vue-router";

const formRef = ref<FormInstance>();
const queryClient = useQueryClient();
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
    typeof route.query.code === "string"
      ? route.query.code
      : route.query.code?.[0] ?? "";

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

const resetPasswordMutation = useMutation(
  (form: IResetPasswordModel) => resetPasswordFn(form),
  {
    onError: (error: any) => {
      ElMessage({
        showClose: true,
        message: "Something went wrong",
        type: "error",
      });
    },
    onSuccess: (data: GenericResponse) => {
      ElMessage({
        showClose: true,
        message: "Password changed successfully!",
        type: "success",
      });
      router.push({ name: "login" });
    },
  }
);

const submitForm = async (formEl: FormInstance | undefined) => {
  if (!formEl) return;
  await formEl.validate((valid, fields) => {
    if (valid) {
      resetPasswordMutation.mutate({
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
  <main class="flex flex-1 justify-center items-center">
    <div>Reset password</div>
    <div class="w-96 border-gray-600 border-2 p-10 rounded-md bg-gray-200">
      <el-form
        label-position="top"
        ref="formRef"
        :model="forgotPasswordModel"
        class="text-right"
        :rules="rules"
      >
        <el-form-item class="text-lg" prop="password">
          <label>Password</label>
          <el-input
            v-model="forgotPasswordModel.newPassword"
            type="password"
            placeholder="Password"
            show-password
            size="large"
          />
        </el-form-item>
        <el-form-item class="text-lg" prop="passwordConfirm">
          <label>Password confirmation</label>
          <el-input
            v-model="forgotPasswordModel.newPasswordConfirm"
            type="password"
            placeholder="Password"
            show-password
            size="large"
          />
        </el-form-item>
        <el-button @click="submitForm(formRef)" class=""
          >Reset password</el-button
        >
      </el-form>
    </div>
  </main>
</template>
