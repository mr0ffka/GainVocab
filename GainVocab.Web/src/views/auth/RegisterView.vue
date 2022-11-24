<script setup lang="ts">
import { onBeforeUpdate, reactive, ref } from "vue";
import { loginUserFn, registerUserFn } from "@/services/authApi";
import type { IRegisterModel } from "@/services/types";
import { ElMessage, FormInstance } from "element-plus";
import { useMutation, useQuery, useQueryClient } from "@tanstack/vue-query";
import router from "@/router";
import { IUserResponse } from "@/services/types";

const formRef = ref<FormInstance>();
const queryClient = useQueryClient();

const registerModel: IRegisterModel = reactive({
  firstName: "",
  lastName: "",
  email: "",
  password: "",
  passwordConfirm: "",
});

const rules = reactive({
  firstName: [{ required: true, message: "First name is required" }],
  lastName: [{ required: true, message: "Last name is required" }],
  email: [{ required: true, message: "Email is required" }],
  password: [{ required: true, message: "Password is required" }],
  passwordConfirm: [
    { required: true, message: "Password confirmation is required" },
  ],
});

const loginMutation = useMutation(
  (credentials: IRegisterModel) => registerUserFn(credentials),
  {
    onError: (error: any) => {
      ElMessage({
        showClose: true,
        message: error.response.data.Title,
        type: "error",
      });
    },
    onSuccess: (data: IUserResponse) => {
      ElMessage({
        showClose: true,
        message: "Registration successful",
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
        firstName: registerModel.firstName,
        lastName: registerModel.lastName,
        email: registerModel.email,
        password: registerModel.password,
        passwordConfirm: registerModel.passwordConfirm,
      });
    } else {
      console.log("error submit!", fields);
    }
  });
};
</script>

<template>
  <main class="flex flex-1 justify-center items-center">
    <div class="w-96 border-gray-600 border-2 p-10 rounded-md bg-gray-200">
      <div class="font-bold text-center mb-5 text-lg">Register new account</div>
      <el-form
        label-position="top"
        ref="formRef"
        :model="registerModel"
        class="text-right"
        :rules="rules"
      >
        <el-form-item prop="email">
          <label for="Email">First name</label>
          <el-input
            v-model="registerModel.firstName"
            placeholder="First name"
            clearable
            size="large"
          />
        </el-form-item>
        <el-form-item prop="email">
          <label for="Email">Last name</label>
          <el-input
            v-model="registerModel.lastName"
            placeholder="Last name"
            clearable
            size="large"
          />
        </el-form-item>
        <el-form-item prop="email">
          <label for="Email">Email</label>
          <el-input
            v-model="registerModel.email"
            placeholder="Email"
            clearable
            size="large"
          />
        </el-form-item>
        <el-form-item class="text-lg" prop="password">
          <label>Password</label>
          <el-input
            v-model="registerModel.password"
            type="password"
            placeholder="Password"
            show-password
            size="large"
          />
        </el-form-item>
        <el-form-item class="text-lg" prop="passwordConfirm">
          <label>Password confirmation</label>
          <el-input
            v-model="registerModel.passwordConfirm"
            type="password"
            placeholder="Password"
            show-password
            size="large"
          />
        </el-form-item>
        <el-button
          size="large"
          @click="submitForm(formRef)"
          class="mt-2 text-center w-1/2 -translate-x-1/2"
        >
          Register new account
        </el-button>
      </el-form>
      <router-link :to="{ name: 'login' }">
        <div class="mt-5 text-center text-sm no-underline hover:underline">
          Has an account already? Log in here!
        </div>
      </router-link>
    </div>
  </main>
</template>
