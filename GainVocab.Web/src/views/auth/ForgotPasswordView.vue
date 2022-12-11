<script setup lang="ts">
import { reactive, ref } from "vue";
import { forgotPasswordFn } from "@/services/auth/authApi";
import type { IForgotPasswordModel } from "@/services/auth/types";
import { ElMessage, FormInstance } from "element-plus";
import router from "@/router";

const formRef = ref<FormInstance>();

const formModel: IForgotPasswordModel = reactive({
  email: "",
});

const rules = reactive({
  email: [{ required: true, message: "Email is required" }],
});

const forgotPassword = (email: IForgotPasswordModel) =>
  forgotPasswordFn(formModel)
    .then((data) => {
      ElMessage({
        showClose: true,
        message: "Reset passowrd link has been send",
        type: "success",
      });
      router.push({ name: "login" });
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

const submitForm = async (formEl: FormInstance | undefined) => {
  if (!formEl) return;
  await formEl.validate((valid, fields) => {
    if (valid) {
      forgotPassword({
        email: formModel.email,
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
      <div class="font-bold text-center mb-5 text-lg">
        Send reset password email
      </div>
      <el-form
        label-position="top"
        ref="formRef"
        :model="formModel"
        class="text-right"
        :rules="rules"
      >
        <el-form-item prop="email">
          <label for="Email">Email</label>
          <el-input
            v-model="formModel.email"
            placeholder="Email"
            clearable
            size="large"
          />
        </el-form-item>
        <el-button size="large" @click="router.go(-1)" class="">
          Go back
        </el-button>
        <el-button size="large" @click="submitForm(formRef)" class="">
          Send
        </el-button>
      </el-form>
    </div>
  </main>
</template>
