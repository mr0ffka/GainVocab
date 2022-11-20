<script setup lang="ts">
import { reactive, ref } from 'vue';
import { forgotPasswordFn, loginUserFn, registerUserFn } from '@/services/authApi';
import type { GenericResponse, IForgotPasswordModel, IRegisterModel } from '@/services/types';
import { ElMessage, FormInstance } from 'element-plus';
import { useMutation, useQueryClient } from '@tanstack/vue-query';
import router from '@/router';

const formRef = ref<FormInstance>();
const queryClient = useQueryClient();

const formModel: IForgotPasswordModel = reactive({
  email: ""
});

const rules = reactive({
  email: [{ required: true, message: "Email is required" }],
});

const forgotPasswordMutation = useMutation(
  (email: IForgotPasswordModel) => forgotPasswordFn(formModel),
  {
    onError: (error: any) => {
      ElMessage({
        showClose: true,
        message: "Something went wrong",
        type: "error"
      });
    },
    onSuccess: (data: GenericResponse) => {
      ElMessage({
        showClose: true,
        message: "Reset passowrd link has been send",
        type: "success"
      });
      router.push({ name: 'login' });
    },
  }
);

const submitForm = async (formEl: FormInstance | undefined) => {
  if (!formEl) return
  await formEl.validate((valid, fields) => {
    if (valid) {
      forgotPasswordMutation.mutate({
        email: formModel.email
      });
    } else {
      console.log('error submit!', fields)
    }
  })
}
</script>

<template>
  <main class="flex flex-1 justify-center items-center">
    <div>Reset password</div>
    <div class="w-96 border-gray-600 border-2 p-10 rounded-md bg-gray-200">
      <el-form label-position="top" ref="formRef" :model="formModel" class="text-right" :rules="rules">
        <el-form-item prop="email">
          <label for="Email">Email</label>
          <el-input v-model="formModel.email" placeholder="Email" clearable size="large" />
          <el-button @click="submitForm(formRef)" class="">Send reset password link</el-button>
        </el-form-item>
      </el-form>
    </div>
  </main>
</template>