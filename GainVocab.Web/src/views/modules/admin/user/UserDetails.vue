<script setup lang="ts">
import AdminMenu from "@/components/admin/AdminMenu.vue";
import { IUserDetailsModel } from "@/services/admin/types";
import { ElMessage, FormInstance } from "element-plus";
import { onMounted, reactive, ref } from "vue";
import router from "@/router";
import { getUserDetails } from "@/services/admin/adminApi";
import { useAdminUserStore } from "@/store/adminUserStore";
import Header from "@/components/common/Header.vue";
import { useRoute } from "vue-router";

const route = useRoute();
const userId = ref<string>("");
const userStore = useAdminUserStore();
const userRoleOptions = userStore.getUserRoleOptions();

const userDetailsModel: IUserDetailsModel = reactive({
  firstName: "",
  lastName: "",
  email: "",
  roles: [],
  courses: [],
});

onMounted(async () => {
  if (route.params.id !== undefined) {
    userId.value = route.params.id.toString();
    userGetDetails();
  }
});

const userGetDetails = () =>
  getUserDetails(userId.value.toString())
    .then((data: IUserDetailsModel) => {
      userDetailsModel.email = data.email;
      userDetailsModel.firstName = data.firstName;
      userDetailsModel.lastName = data.lastName;
      userDetailsModel.roles = data.roles;
      userDetailsModel.courses = data.courses;
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
</script>

<template>
  <Header></Header>
  <div class="flex grow">
    <AdminMenu />
    <div class="grow flex flex-col p-2">
      <div class="flex">
        <span class="font-bold text-xl">User details</span>
      </div>
      <el-descriptions
        v-if="userDetailsModel.firstName != ''"
        :column="1"
        :border="true"
        class="mt-2"
      >
        <el-descriptions-item label="First name">{{
          userDetailsModel.firstName
        }}</el-descriptions-item>
        <el-descriptions-item label="Last name">{{
          userDetailsModel.lastName
        }}</el-descriptions-item>
        <el-descriptions-item label="Email">{{
          userDetailsModel.email
        }}</el-descriptions-item>
        <el-descriptions-item label="Roles">
          <el-tag
            v-for="role in userDetailsModel.roles"
            class="mr-2"
            disable-transitions
            >{{ role }}
          </el-tag>
        </el-descriptions-item>
        <el-descriptions-item label="Courses"
          ><el-tag
            v-for="course in userDetailsModel.courses"
            class="mr-2"
            disable-transitions
            >{{ course }}
          </el-tag>
        </el-descriptions-item>
      </el-descriptions>
      <div class="mt-2">
        <el-button
          size="large"
          @click="router.go(-1)"
          type="info"
          plain
          class="mr-2 text-center !ml-auto"
        >
          Go back
        </el-button>
      </div>
    </div>
  </div>
</template>

<style scoped></style>
