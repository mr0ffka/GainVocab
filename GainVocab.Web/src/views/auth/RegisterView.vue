<script setup lang="ts">
import { onBeforeUpdate, reactive, ref } from 'vue';
import { loginUserFn, registerUserFn } from '@/services/authApi';
import type { IRegisterModel } from '@/services/types';
import { ElMessage, FormInstance } from 'element-plus';
import { UserRolesEnum } from '@/helpers/enums/UserRolesEnum';
import { useMutation, useQuery, useQueryClient } from '@tanstack/vue-query';
import router from '@/router';
import { IUserResponse } from '@/services/types';
import { useAuthUser } from '@/services/authQueries';
import { CallbackTypes, GoogleLogin } from 'vue3-google-login';

const formRef = ref<FormInstance>();
const queryClient = useQueryClient();

const registerModel: IRegisterModel = reactive({
    firstName: "",
    lastName: "",
    email: "",
    password: "",
    passwordConfirm: "",
})

const rules = reactive({
    firstName: [{ required: true, message: "First name is required" }],
    lastName: [{ required: true, message: "Last name is required" }],
    email: [{ required: true, message: "Email is required" }],
    password: [{ required: true, message: "Password is required" }],
    passwordConfirm: [{ required: true, message: "Password confirmation is required" }],
});

const loginMutation = useMutation(
    (credentials: IRegisterModel) => registerUserFn(credentials),
    {
        onError: (error: any) => {
            let message = "";
            console.log("dupa");
            if ((error as any).response.data.title != null) {
                message = (error as any).response.data.title;
            }
            else {
                message = (error as any).response.data[0].description;
            }
            ElMessage({
                showClose: true,
                message: message,
                type: "error"
            });
        },
        onSuccess: (data: IUserResponse) => {
            ElMessage({
                showClose: true,
                message: "Registration successful",
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
                firstName: registerModel.first_name,
                lastName: registerModel.last_name,
                email: registerModel.email,
                password: registerModel.password,
                passwordConfirm: registerModel.password_confirm,
            });
        } else {
            console.log('error submit!', fields)
        }
    })
}
</script>

<template>
    <main class="flex flex-1 justify-center items-center">
        <div>Register new account</div>
        <div class="w-96 border-gray-600 border-2 p-10 rounded-md bg-gray-200">
            <el-form label-position="top" ref="formRef" :model="registerModel" class="text-right" :rules="rules">
                <el-form-item prop="email">
                    <label for="Email">First name</label>
                    <el-input v-model="registerModel.first_name" placeholder="First name" clearable size="large" />
                </el-form-item>
                <el-form-item prop="email">
                    <label for="Email">Last name</label>
                    <el-input v-model="registerModel.last_name" placeholder="Last name" clearable size="large" />
                </el-form-item>
                <el-form-item prop="email">
                    <label for="Email">Email</label>
                    <el-input v-model="registerModel.email" placeholder="Email" clearable size="large" />
                </el-form-item>
                <el-form-item class="text-lg" prop="password">
                    <label>Password</label>
                    <el-input v-model="registerModel.password" type="password" placeholder="Password" show-password
                        size="large" />
                </el-form-item>
                <el-form-item class="text-lg" prop="password_confirm">
                    <label>Password confirmation</label>
                    <el-input v-model="registerModel.password_confirm" type="password" placeholder="Password"
                        show-password size="large" />
                </el-form-item>
                <el-button @click="submitForm(formRef)" class="">Register new account</el-button>
            </el-form>
        </div>
    </main>
</template>