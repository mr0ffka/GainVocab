<script setup lang="ts">
import { reactive, ref } from "vue";
import { loginUserFn } from "@/services/auth/authApi";
import type { ILoginModel } from "@/services/auth/types";
import { ElMessage, FormInstance } from "element-plus";
import { useQueryClient } from "@tanstack/vue-query";
import { router } from "@/router";
import { IUserAuthResponse } from "@/services/auth/types";

const formRef = ref<FormInstance>();
const queryClient = useQueryClient();
const isLogging = ref(false);

const loginModel: ILoginModel = reactive({
  email: "",
  password: "",
  rememberMe: false,
});

const rules = reactive({
  email: [{ required: true, message: "Email is required" }],
  password: [{ required: true, message: "Password is required" }],
});

const login = async (credentials: ILoginModel) =>
  loginUserFn(credentials)
    .then(async (data: IUserAuthResponse) => {
      ElMessage({
        showClose: true,
        message: "Successfully logged in",
        type: "success",
      });
      await queryClient.setQueryData(["authUser"], data.user);
      if (data.user.isAdmin) {
        router.push({ name: "admin-dashboard" });
      } else {
        router.push({ name: "user-dashboard" });
      }
    })
    .catch((error: any) => {
      error.response.data.Errors.forEach(async (e: any) => {
        ElMessage({
          showClose: true,
          message: e.Title,
          type: "error",
        });
      });
      isLogging.value = false;
    });

const submitForm = async (formEl: FormInstance | undefined) => {
  isLogging.value = true;
  if (!formEl) return;
  await formEl.validate((valid, fields) => {
    if (valid) {
      login({
        email: loginModel.email,
        password: loginModel.password,
        rememberMe: loginModel.rememberMe,
      });
    } else {
      console.log("error submit!", fields);
      isLogging.value = false;
    }
  });
};
</script>

<template>
  <main class="flex justify-center items-center h-screen drop-shadow">
    <div
      class="border-grey border p-10 xs:w-1 sm:w-4/5 md:w-3/5 lg:w-2/5 xl:w-2/5 2xl:w-1/5 3xl:w-1/10 shadow-md"
    >
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
          <el-button
            size="large"
            :loading="isLogging"
            @click="submitForm(formRef)"
            class="!ml-auto min-w-full"
            >Login</el-button
          >
        </el-form-item>
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
