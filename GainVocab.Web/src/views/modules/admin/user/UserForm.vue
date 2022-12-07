<script setup lang="ts">
import AdminMenu from "@/components/admin/AdminMenu.vue";
import { IUserAddModel, USER_ROLES } from "@/services/user/types";
import { useMutation } from "@tanstack/vue-query";
import { ElMessage, FormInstance } from "element-plus";
import { reactive, ref } from "vue";
import router from "@/router";
import { addUser } from "@/services/user/userApi";
import { ErrorResponse } from "@/services/common/types";
import { useUserStore } from "@/store/userStore";

const formRef = ref<FormInstance>();
const userStore = useUserStore();
const userRoleKeyValuePair = userStore.getUserRoleKeyValuePair();

const userAddModel: IUserAddModel = reactive({
  firstName: "",
  lastName: "",
  email: "",
  password: "",
  passwordConfirm: "",
  roles: [],
});

const rules = reactive({
  firstName: [{ required: true, message: "First name is required" }],
  lastName: [{ required: true, message: "Last name is required" }],
  email: [{ required: true, message: "Email is required" }],
  password: [{ required: true, message: "Password is required" }],
  passwordConfirm: [
    { required: true, message: "Password confirmation is required" },
  ],
  roles: [{ required: true, message: "Select a user role" }],
});

const userAdd = (user: IUserAddModel) =>
  addUser(user)
    .then(() => {
      ElMessage({
        showClose: true,
        message: "User added",
        type: "success",
      });
      router.push({ name: "user-list" });
    })
    .catch((error: any) => {
      let message: string = "";
      error.errors.forEach(async (e: any) => {
        message += e.title + "<br/>";
      });
      console.log(message);
      ElMessage({
        showClose: true,
        message: message,
        type: "error",
      });
    });

const submitForm = async (formEl: FormInstance | undefined) => {
  if (!formEl) return;
  await formEl.validate((valid, fields) => {
    if (valid) {
      userAdd({
        firstName: userAddModel.firstName,
        lastName: userAddModel.lastName,
        email: userAddModel.email,
        password: userAddModel.password,
        passwordConfirm: userAddModel.passwordConfirm,
        roles: userAddModel.roles,
      });
    } else {
      console.log("error submit!", fields);
    }
  });
};
</script>

<template>
  <div class="flex grow">
    <AdminMenu />
    <div class="grow flex flex-col p-2">
      <div class="flex">
        <span class="font-bold text-xl">Add new user</span>
      </div>
      <el-form
        class="my-2"
        label-position="left"
        label-width="auto"
        ref="formRef"
        :model="userAddModel"
        :rules="rules"
      >
        <el-form-item prop="firstName" label="First name">
          <el-input
            v-model="userAddModel.firstName"
            placeholder="First name"
            clearable
            size="large"
          />
        </el-form-item>
        <el-form-item prop="lastName" label="Last name">
          <el-input
            v-model="userAddModel.lastName"
            placeholder="Last name"
            clearable
            size="large"
          />
        </el-form-item>
        <el-form-item prop="email" label="Email">
          <el-input
            v-model="userAddModel.email"
            placeholder="Email"
            clearable
            size="large"
          />
        </el-form-item>
        <el-form-item class="text-lg" prop="password" label="Password">
          <el-input
            v-model="userAddModel.password"
            type="password"
            placeholder="Password"
            show-password
            size="large"
          />
        </el-form-item>
        <el-form-item
          class="text-lg"
          prop="passwordConfirm"
          label="Password confirmation"
        >
          <el-input
            v-model="userAddModel.passwordConfirm"
            type="password"
            placeholder="Password"
            show-password
            size="large"
          />
        </el-form-item>
        <el-form-item class="text-lg" prop="roles" label="User roles">
          <el-select
            v-model="userAddModel.roles"
            multiple
            collapse-tags-tooltip
            placeholder="User roles"
            size="large"
            clearable
            class="grow"
          >
            <el-option
              v-for="item in userRoleKeyValuePair"
              :key="item.key"
              :label="item.value"
              :value="item.key"
            />
          </el-select>
        </el-form-item>
      </el-form>
      <div>
        <el-button
          size="large"
          @click="router.go(-1)"
          type="info"
          plain
          class="mr-2 text-center !ml-auto"
        >
          Go back
        </el-button>
        <el-button
          size="large"
          type="success"
          plain
          @click="submitForm(formRef)"
          class="text-center !ml-auto"
        >
          Add new account
        </el-button>
      </div>
    </div>
  </div>
</template>

<style scoped></style>
