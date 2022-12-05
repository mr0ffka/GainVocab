<script setup lang="ts">
import { onBeforeUpdate, reactive, ref } from "vue";
import { loginUserFn } from "@/services/auth/authApi";
import type { ILoginModel } from "@/services/auth/types";
import { ElMessage, FormInstance } from "element-plus";
import { useMutation, useQueryClient } from "@tanstack/vue-query";
import router from "@/router";
import { IUserAuthResponse } from "@/services/auth/types";
import { CallbackTypes } from "vue3-google-login";

const formRef = ref<FormInstance>();
const queryClient = useQueryClient();

const loginModel: ILoginModel = reactive({
  email: "",
  password: "",
  rememberMe: false,
});

const rules = reactive({
  email: [{ required: true, message: "Email is required" }],
  password: [{ required: true, message: "Password is required" }],
});

const loginMutation = useMutation(
  (credentials: ILoginModel) => loginUserFn(credentials),
  {
    onError: (error: any) => {
      ElMessage({
        showClose: true,
        message: error.response.data.Title,
        type: "error",
      });
    },
    onSuccess: (data: IUserAuthResponse) => {
      ElMessage({
        showClose: true,
        message: "Successfully logged in",
        type: "success",
      });
      queryClient.setQueryData(["authUser"], data.user);
      if (data.user.isAdmin) {
        router.push({ name: "admin-dashboard" });
      } else {
        router.push({ name: "user-dashboard" });
      }
    },
  }
);

const submitForm = async (formEl: FormInstance | undefined) => {
  if (!formEl) return;
  await formEl.validate((valid, fields) => {
    if (valid) {
      loginMutation.mutate({
        email: loginModel.email,
        password: loginModel.password,
        rememberMe: loginModel.rememberMe,
      });
    } else {
      console.log("error submit!", fields);
    }
  });
};

const googleLoginCallback: CallbackTypes.CredentialCallback = (response) => {
  // axios
};
</script>

<template>
  <main class="flex flex-1 justify-center items-center">
    <div class="w-96 border-gray-600 border-2 p-10 rounded-md bg-gray-200">
      <div class="font-bold text-center mb-5 text-lg">
        Login into an account
      </div>
      <el-form
        label-position="top"
        ref="formRef"
        :model="loginModel"
        class="text-right"
        :rules="rules"
      >
        <el-form-item prop="email">
          <label for="Email">Email</label>
          <el-input
            v-model="loginModel.email"
            placeholder="Email"
            clearable
            size="large"
          />
        </el-form-item>
        <el-form-item class="text-lg" prop="password">
          <label>Password</label>
          <el-input
            v-model="loginModel.password"
            type="password"
            placeholder="Password"
            show-password
            size="large"
          />
        </el-form-item>
        <el-form-item>
          <el-checkbox
            v-model="loginModel.rememberMe"
            label="Remember me"
            size="large"
          />
          <el-button size="large" @click="submitForm(formRef)" class="!ml-auto"
            >Login</el-button
          >
        </el-form-item>
        <!-- <google-login :callback="googleLoginCallback" /> -->
      </el-form>
      <div class="flex">
        <router-link :to="{ name: 'register' }">
          <span class="text-sm no-underline hover:underline !ml-auto">
            Create new account
          </span>
        </router-link>
        <router-link :to="{ name: 'forgotpassword' }" class="!ml-auto">
          <span class="text-sm no-underline hover:underline"
            >Forgot your credentials?
          </span>
        </router-link>
      </div>
    </div>
  </main>
</template>

<style scoped></style>
