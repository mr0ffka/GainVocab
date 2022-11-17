<script setup lang="ts">
import { onBeforeUpdate, reactive, ref } from 'vue';
import { loginUserFn } from '@/services/authApi';
import type { ILoginModel } from '@/services/types';
import { ElMessage, FormInstance } from 'element-plus';
import { UserRolesEnum } from '@/helpers/enums/UserRolesEnum';
import { useMutation, useQuery, useQueryClient } from '@tanstack/vue-query';
import router from '@/router';
import { IUserResponse } from '@/services/types';
import { useAuthUser } from '@/services/authQueries';
import { CallbackTypes, GoogleLogin } from 'vue3-google-login';

const formRef = ref<FormInstance>();
const queryClient = useQueryClient();

const loginModel: ILoginModel = reactive({
  email: "",
  password: ""
})

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
        message: (error as any).response.data.title,
        type: "error"
      });
    },
    onSuccess: (data: IUserResponse) => {
      ElMessage({
        showClose: true,
        message: "Successfully logged in",
        type: "success"
      });
      queryClient.setQueryData(['authUser'], data.user);
      if (data.user.isAdmin) {
        router.push({ name: 'admin-dashboard' });
      } else {
        router.push({ name: 'user-dashboard' });
      }
    },
  }
);

const submitForm = async (formEl: FormInstance | undefined) => {
  if (!formEl) return
  await formEl.validate((valid, fields) => {
    if (valid) {
      loginMutation.mutate({
        email: loginModel.email,
        password: loginModel.password,
      });
    } else {
      console.log('error submit!', fields)
    }
  })
}

const googleLoginCallback: CallbackTypes.CredentialCallback = (response) => {
  // axios
}
</script>

<template>
  <main class="flex flex-1 justify-center items-center">
    <div>Login into an account</div>
    <div class="w-96 border-gray-600 border-2 p-10 rounded-md bg-gray-200">
      <el-form label-position="top" ref="formRef" :model="loginModel" class="text-right" :rules="rules">
        <el-form-item prop="email">
          <label for="Email">Email</label>
          <el-input v-model="loginModel.email" placeholder="Email" clearable size="large" />
        </el-form-item>
        <el-form-item class="text-lg" prop="password">
          <label>Password</label>
          <el-input v-model="loginModel.password" type="password" placeholder="Password" show-password size="large" />
        </el-form-item>
        <el-button @click="submitForm(formRef)" class="">Login</el-button>
        <!-- <google-login :callback="googleLoginCallback" /> -->
      </el-form>
    </div>
  </main>
</template>