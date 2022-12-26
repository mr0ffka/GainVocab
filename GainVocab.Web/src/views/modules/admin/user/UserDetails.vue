<script setup lang="ts">
import AdminMenu from "@/components/admin/AdminMenu.vue";
import { IUserDetailsModel } from "@/services/admin/types";
import { ElMessage } from "element-plus";
import { onMounted, reactive, ref } from "vue";
import router from "@/router";
import { getUserDetails, removeUser } from "@/services/admin/adminApi";
import Header from "@/components/common/Header.vue";
import { useRoute } from "vue-router";

const route = useRoute();
const userId = ref<string>("");
const hasData = ref(false);
const confirmDeleteDialog = ref(false);

const userDetailsModel: IUserDetailsModel = reactive({
  firstName: "",
  lastName: "",
  email: "",
  emailConfirmed: false,
  roles: [],
  courses: [],
  coursesDone: [],
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
      userDetailsModel.emailConfirmed = data.emailConfirmed;
      userDetailsModel.firstName = data.firstName;
      userDetailsModel.lastName = data.lastName;
      userDetailsModel.roles = data.roles;
      userDetailsModel.courses = data.courses;
      hasData.value = true;
    })
    .catch((error: any) => {
      hasData.value = false;
      error.response.data.Errors.forEach(async (e: any) => {
        ElMessage({
          showClose: true,
          message: e.Title,
          type: "error",
        });
      });
    });

const deleteUser = async (id: string) =>
  await removeUser(id)
    .then((data: any) => {
      ElMessage({
        showClose: true,
        message: "User has been deleted",
        type: "success",
      });
      router.push({ name: "admin-user-list" });
    })
    .catch((error: any) => {
      ElMessage({
        showClose: true,
        message: error.response.data.Title,
        type: "error",
      });
    });

const handleEdit = () => {
  router.push({ name: "user-edit", params: { id: userId.value.toString() } });
};
const handleDeleteDialog = () => {
  confirmDeleteDialog.value = true;
};
const handleDelete = () => {
  deleteUser(userId.value.toString());
  confirmDeleteDialog.value = false;
};
</script>

<template>
  <Header></Header>
  <div class="flex grow">
    <AdminMenu />
    <div class="grow flex flex-col p-2">
      <div class="flex">
        <span class="font-bold text-xl">User details</span>
      </div>
      <el-descriptions v-if="hasData" :column="1" :border="true" class="mt-2">
        <el-descriptions-item label="First name">{{
          userDetailsModel.firstName
        }}</el-descriptions-item>
        <el-descriptions-item label="Last name">{{
          userDetailsModel.lastName
        }}</el-descriptions-item>
        <el-descriptions-item label="Email">{{
          userDetailsModel.email
        }}</el-descriptions-item>
        <el-descriptions-item label="Email confirmed">
          <el-tag disable-transitions>
            {{ userDetailsModel.emailConfirmed ? "Yes" : "No" }}
          </el-tag>
        </el-descriptions-item>
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
            v-if="userDetailsModel.courses.length > 0"
            v-for="course in userDetailsModel.courses"
            class="mr-2"
            disable-transitions
            >{{ course }}
          </el-tag>
          <el-tag v-else class="mr-2" disable-transitions type="danger">
            None
          </el-tag>
        </el-descriptions-item>
      </el-descriptions>
      <div v-else class="flex">
        <span class="font-bold text-center">No data</span>
      </div>
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
        <el-button
          v-if="hasData"
          size="large"
          @click="handleEdit"
          plain
          class="mr-2 text-center !ml-auto"
        >
          Edit
        </el-button>
        <el-button
          v-if="hasData"
          size="large"
          class="mr-2 text-center !ml-auto"
          type="danger"
          plain
          @click="handleDeleteDialog"
          >Delete</el-button
        >
      </div>
    </div>
  </div>
  <el-dialog
    v-model="confirmDeleteDialog"
    title="Delete user"
    width="30%"
    center
  >
    Do you really want to delete user:
    <span class="font-bold">
      {{ userDetailsModel.firstName }} {{ userDetailsModel.lastName }}</span
    >?
    <template #footer>
      <span class="dialog-footer">
        <el-button plain @click="confirmDeleteDialog = false">Cancel</el-button>
        <el-button type="danger" plain @click="handleDelete">
          Delete
        </el-button>
      </span>
    </template>
  </el-dialog>
</template>

<style scoped></style>
