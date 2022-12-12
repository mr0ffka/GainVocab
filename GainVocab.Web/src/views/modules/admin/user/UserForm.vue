<script setup lang="ts">
import AdminMenu from "@/components/admin/AdminMenu.vue";
import { IUserAddModel } from "@/services/admin/types";
import { ElMessage, FormInstance } from "element-plus";
import { onMounted, reactive, ref } from "vue";
import router from "@/router";
import { addUser, getUser, updateUser } from "@/services/admin/adminApi";
import { useAdminUserStore } from "@/store/adminUserStore";
import Header from "@/components/common/Header.vue";
import { useRoute } from "vue-router";

const route = useRoute();
const userId = ref<string>("");
const formRef = ref<FormInstance>();
const userStore = useAdminUserStore();
const userRoleOptions = userStore.getUserRoleOptions();

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

onMounted(async () => {
  if (route.params.id !== undefined) {
    userId.value = route.params.id.toString();
    userGet();
  }
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
      error.response.data.Errors.forEach(async (e: any) => {
        ElMessage({
          showClose: true,
          message: e.Title,
          type: "error",
        });
      });
    });

const userGet = () =>
  getUser(userId.value.toString())
    .then((data: IUserAddModel) => {
      userAddModel.email = data.email;
      userAddModel.firstName = data.firstName;
      userAddModel.lastName = data.lastName;
      userAddModel.roles = data.roles;
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

const userUpdate = (model: IUserAddModel) =>
  updateUser(userId.value.toString(), model)
    .then(() => {
      ElMessage({
        showClose: true,
        message: "User updated",
        type: "success",
      });
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
      if (userId.value === "" || userId.value === null) {
        userAdd({
          firstName: userAddModel.firstName,
          lastName: userAddModel.lastName,
          email: userAddModel.email,
          password: userAddModel.password,
          passwordConfirm: userAddModel.passwordConfirm,
          roles: userAddModel.roles,
        });
      } else {
        userUpdate({
          firstName: userAddModel.firstName,
          lastName: userAddModel.lastName,
          email: userAddModel.email,
          password: userAddModel.password,
          passwordConfirm: userAddModel.passwordConfirm,
          roles: userAddModel.roles,
        });
      }
    } else {
      console.log("error submit!", fields);
    }
  });
};
</script>

<template>
  <Header></Header>
  <div class="flex grow">
    <AdminMenu />
    <div class="grow flex flex-col p-2">
      <div class="flex">
        <span v-if="userId === '' || userId === null" class="font-bold text-xl"
          >Add new user</span
        >
        <span v-else class="font-bold text-xl">Edit user</span>
      </div>
      <el-form
        class="my-2"
        label-position="left"
        label-width="auto"
        ref="formRef"
        :model="userAddModel"
        :rules="userId === '' || userId === null ? rules : {}"
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
            :disabled="userId !== ''"
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
              v-for="item in userRoleOptions"
              :key="item"
              :label="item"
              :value="item"
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
          <span v-if="userId === '' || userId === null">Add new user</span>
          <span v-else>Edit user</span>
        </el-button>
      </div>
    </div>
  </div>
</template>

<style scoped></style>
