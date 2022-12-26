<script setup lang="ts">
import UserMenu from "@/components/user/UserMenu.vue";
import { IUserDetailsModel } from "@/services/admin/types";
import { ElMessage, FormInstance } from "element-plus";
import { onMounted, reactive, ref } from "vue";
import router from "@/router";
import Header from "@/components/common/Header.vue";
import { useQuery } from "@tanstack/vue-query";
import { getCurrUser } from "@/services/auth/authApi";
import { getMeDetails, removeMe, updateMe } from "@/services/user/userApi";
import { IUserProfileEditModel } from "@/services/user/types";

const userId = ref<string>("");
const hasData = ref(false);
const confirmDeleteDialog = ref(false);
const userProfileEditDialog = ref(false);
const changePassword = ref(false);
const auth = useQuery(["authUser"], getCurrUser);
const formProfileEditRef = ref<FormInstance>();

const rulesEdit = reactive({
  firstName: [{ required: true, message: "First name is required" }],
  lastName: [{ required: true, message: "Last name is required" }],
});

const rulesEditWithPassword = reactive({
  firstName: [{ required: true, message: "First name is required" }],
  lastName: [{ required: true, message: "Last name is required" }],
  currentPassword: [
    { required: true, message: "Current password is required" },
  ],
  password: [{ required: true, message: "New password is required" }],
  passwordConfirm: [
    { required: true, message: "New password confirmation is required" },
  ],
});

const userDetailsModel: IUserDetailsModel = reactive({
  firstName: "",
  lastName: "",
  email: "",
  emailConfirmed: false,
  roles: [],
  courses: [],
  coursesDone: [],
});

const userProfileEditModel: IUserProfileEditModel = reactive({
  firstName: "",
  lastName: "",
  currentPassword: "",
  password: "",
  passwordConfirm: "",
  courses: [],
});

onMounted(() => {
  userId.value = auth.data.value?.id != undefined ? auth.data.value?.id : "";
  if (userId.value != "") {
    userGetDetails();
  }
});

const userGetDetails = () =>
  getMeDetails(userId.value)
    .then((data: IUserDetailsModel) => {
      userDetailsModel.email = data.email;
      userDetailsModel.emailConfirmed = data.emailConfirmed;
      userDetailsModel.firstName = data.firstName;
      userDetailsModel.lastName = data.lastName;
      userDetailsModel.roles = data.roles;
      userDetailsModel.courses = data.courses;
      userDetailsModel.coursesDone = data.coursesDone;
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

const deleteMe = async (id: string) =>
  await removeMe(id)
    .then(async (data: any) => {
      ElMessage({
        showClose: true,
        message: "User has been deleted",
        type: "success",
      });
      router.go(0);
    })
    .catch((error: any) => {
      ElMessage({
        showClose: true,
        message: error.response.data.Title,
        type: "error",
      });
    });

const handleDeleteDialog = () => {
  confirmDeleteDialog.value = true;
};
const handleDelete = () => {
  deleteMe(userId.value.toString());
  confirmDeleteDialog.value = false;
};
const handleEditDialog = () => {
  userProfileEditModel.firstName = userDetailsModel.firstName;
  userProfileEditModel.lastName = userDetailsModel.lastName;
  userProfileEditModel.courses = userDetailsModel.courses;
  userProfileEditDialog.value = true;
};

const updateProfile = (model: IUserProfileEditModel) =>
  updateMe(userId.value.toString(), model)
    .then(() => {
      ElMessage({
        showClose: true,
        message: "Profile updated",
        type: "success",
      });
      userProfileEditDialog.value = false;
      router.go(0);
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

const submitEditProfile = async (formEl: FormInstance | undefined) => {
  if (!formEl) return;
  await formEl.validate((valid, fields) => {
    if (valid) {
      updateProfile({
        firstName: userProfileEditModel.firstName,
        lastName: userProfileEditModel.lastName,
        currentPassword: userProfileEditModel.currentPassword,
        password: userProfileEditModel.password,
        passwordConfirm: userProfileEditModel.passwordConfirm,
        courses: userProfileEditModel.courses,
      });
    } else {
      console.log("error submit!", fields);
    }
  });
};
</script>

<template>
  <Header></Header>
  <div class="flex grow">
    <UserMenu />
    <div class="grow flex flex-col p-2">
      <div class="flex">
        <span class="font-bold text-xl"
          >Profile:
          {{
            userDetailsModel.firstName + " " + userDetailsModel.lastName
          }}</span
        >
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
        <el-descriptions-item label="Courses"
          ><el-tag
            v-if="userDetailsModel.courses.length > 0"
            v-for="course in userDetailsModel.courses"
            class="mr-2"
            disable-transitions
            >{{ course }}
          </el-tag>
          <el-tag v-else type="danger">None</el-tag>
        </el-descriptions-item>
        <el-descriptions-item label="Finished courses"
          ><el-tag
            v-if="userDetailsModel.coursesDone.length > 0"
            v-for="course in userDetailsModel.coursesDone"
            class="mr-2"
            type="success"
            disable-transitions
            >'{{ course.courseName }}' with
            <span class="text-red-500">{{ course.amountOfErrors }} errors</span>
          </el-tag>
          <el-tag v-else type="danger">None</el-tag>
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
          @click="handleEditDialog"
          plain
          class="mr-2 text-center !ml-auto"
        >
          Settings
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
    title="Delete your account"
    width="30%"
    center
  >
    Do you really want to delete your account with all your progress?
    <template #footer>
      <span class="dialog-footer">
        <el-button plain @click="confirmDeleteDialog = false">Cancel</el-button>
        <el-button type="danger" plain @click="handleDelete">
          Delete
        </el-button>
      </span>
    </template>
  </el-dialog>

  <el-dialog
    v-model="userProfileEditDialog"
    title="Settings"
    width="30%"
    center
  >
    <el-form
      class="my-2"
      label-position="left"
      label-width="auto"
      ref="formProfileEditRef"
      :model="userProfileEditModel"
      :rules="changePassword ? rulesEditWithPassword : rulesEdit"
    >
      <el-form-item prop="firstName" label="First name">
        <el-input
          v-model="userProfileEditModel.firstName"
          placeholder="First name"
          clearable
          size="large"
        />
      </el-form-item>
      <el-form-item prop="lastName" label="Last name">
        <el-input
          v-model="userProfileEditModel.lastName"
          placeholder="Last name"
          clearable
          size="large"
        />
      </el-form-item>
      <el-form-item class="text-lg" prop="courses" label="Courses">
        <el-select
          v-model="userProfileEditModel.courses"
          multiple
          collapse-tags-tooltip
          placeholder="Courses"
          size="large"
          clearable
          class="grow"
        >
          <el-option
            v-for="item in userDetailsModel.courses"
            :key="item"
            :label="item"
            :value="item"
          />
        </el-select>
      </el-form-item>
      <el-checkbox
        v-model="changePassword"
        label="Change password"
        size="large"
      />
      <div v-if="changePassword">
        <el-form-item
          class="text-lg"
          prop="currentPassword"
          label="Current password"
        >
          <el-input
            v-model="userProfileEditModel.currentPassword"
            type="password"
            placeholder="Current password"
            show-password
            size="large"
          />
        </el-form-item>
        <el-form-item class="text-lg" prop="password" label="New password">
          <el-input
            v-model="userProfileEditModel.password"
            type="password"
            placeholder="New password"
            show-password
            size="large"
          />
        </el-form-item>
        <el-form-item
          class="text-lg"
          prop="passwordConfirm"
          label="New password confirmation"
        >
          <el-input
            v-model="userProfileEditModel.passwordConfirm"
            type="password"
            placeholder="New password confirmation"
            show-password
            size="large"
          />
        </el-form-item>
      </div>
    </el-form>
    <template #footer>
      <span class="dialog-footer">
        <el-button plain @click="userProfileEditDialog = false"
          >Cancel</el-button
        >
        <el-button
          type="success"
          plain
          @click="submitEditProfile(formProfileEditRef)"
          >Edit</el-button
        >
      </span>
    </template>
  </el-dialog>
</template>

<style scoped></style>
