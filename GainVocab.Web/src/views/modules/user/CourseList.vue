<script setup lang="ts">
import UserMenu from "@/components/user/UserMenu.vue";
import { RefreshRight, Search } from "@element-plus/icons-vue";
import Header from "@/components/common/Header.vue";
import { getLanguageOptionsList } from "@/services/admin/adminApi";
import {
  getAvailableCourseList,
  addUserToCourse,
} from "@/services/user/userApi";
import { ElMessage } from "element-plus";
import { storeToRefs } from "pinia";
import { onMounted, ref } from "vue";
import { ICourseListModel, ILanguageModel } from "@/services/admin/types";
import { useUserCourseStore } from "@/store/user/userCourseStore";
import { IPagedResult, IPager } from "@/services/common/types";
import { getCurrUser } from "@/services/auth/authApi";
import { useQuery } from "@tanstack/vue-query";
import { IAddUserToCourseModel } from "@/services/user/types";

const store = useUserCourseStore();
const userId = ref<string>("");
const { filter, pager, isSearching, descriptionLength } = storeToRefs(store);
const languageOptions = ref<ILanguageModel[] | null>();
const focusedItem = ref<ICourseListModel>();
let entities = ref<ICourseListModel[]>();
const auth = useQuery(["authUser"], getCurrUser);
const courseDescriptionDialog = ref(false);
const courseDescriptionDialogTitle = ref("");

const getLanguageOptions = async () => {
  await getLanguageOptionsList()
    .then((data: ILanguageModel[]) => {
      let options: ILanguageModel[] = [];
      data.forEach((el) => {
        options.push(el);
      });
      languageOptions.value = options;
    })
    .catch((error) => {
      isSearching.value = false;
      error.response.data.Errors.forEach(async (e: any) => {
        ElMessage({
          showClose: true,
          message: e.Title,
          type: "error",
        });
      });
    });
};

onMounted(async () => {
  userId.value = auth.data.value?.id != undefined ? auth.data.value?.id : "";
  if (userId.value != "") {
    await getEntities();
  }
  getLanguageOptions();
});

const addToCourse = (model: IAddUserToCourseModel) => {
  addUserToCourse(model)
    .then(() => {
      ElMessage({
        showClose: true,
        message: `You joined course "${focusedItem.value?.name}" successfuly`,
        type: "success",
      });
      getEntities();
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
};

const getEntities = () => {
  isSearching.value = true;
  getAvailableCourseList(userId.value, filter.value)
    .then((data: any) => {
      isSearching.value = false;
      entities.value = data;
    })
    .catch((error) => {
      isSearching.value = false;
      error.response.data.Errors.forEach(async (e: any) => {
        ElMessage({
          showClose: true,
          message: e.Title,
          type: "error",
        });
      });
    });
};

const cutDescription = (desc: string) => {
  return desc.length > descriptionLength.value
    ? desc.substring(0, descriptionLength.value - 3) + "..."
    : desc;
};

const resetFilters = () => {
  store.resetFilters();
  getEntities();
};

const handleDescriptionDialog = (course: ICourseListModel) => {
  courseDescriptionDialog.value = true;
  courseDescriptionDialogTitle.value = `Course: ${course.name}`;
  focusedItem.value = course;
};

const handleJoiningToCourse = (course: ICourseListModel) => {
  focusedItem.value = course;
  addToCourse({ userId: userId.value, coursePublicId: course.id });
  getEntities();
};
</script>

<template>
  <Header></Header>
  <div class="flex grow">
    <UserMenu />
    <div class="grow flex flex-col p-2">
      <div class="flex">
        <div class="flex flex-row">
          <el-input
            v-model="filter.name"
            class="mb-2"
            placeholder="Course name"
          />
          <el-select
            v-model="filter.languageFrom"
            multiple
            collapse-tags
            collapse-tags-tooltip
            placeholder="Language from"
            clearable
            @clear="getEntities"
            class="ml-2 min-w-fit"
          >
            <el-option
              v-for="item in languageOptions"
              :key="item.id"
              :label="item.name"
              :value="item.id"
            />
          </el-select>
          <el-select
            v-model="filter.languageTo"
            multiple
            collapse-tags
            collapse-tags-tooltip
            placeholder="Language to"
            clearable
            @clear="getEntities"
            class="ml-2 min-w-fit"
          >
            <el-option
              v-for="item in languageOptions"
              :key="item.id"
              :label="item.name"
              :value="item.id"
            />
          </el-select>
        </div>
        <div class="flex flex-row !ml-auto">
          <el-button
            class="mb-2 mr-2 p-3 font-bold right !ml-auto"
            plain
            :loading="isSearching"
            @click="getEntities"
            ><el-icon><Search /></el-icon>&nbsp;Search</el-button
          >
          <el-button
            class="mb-2 mr-2 p-3 font-bold right !ml-auto"
            plain
            type="info"
            @click="resetFilters"
            ><el-icon><RefreshRight /></el-icon>&nbsp;Reset filters</el-button
          >
        </div>
      </div>
      <div
        class="grow grid 2xl:grid-cols-4 xl:grid-cols-3 md:grid-cols-2 grid-rows-2 gap-2"
      >
        <el-card
          v-for="course in entities"
          shadow="hover"
          class="box-card"
          body-style="display: flex; flex-flow:column; height: 85%;"
        >
          <template #header>
            <div class="card-header text-center">
              <span class="font-bold">{{ course.name }}</span>
            </div>
          </template>
          <div>
            <div>
              <span>Languages:</span>
            </div>
            <span class="font-bold"
              >{{ course.languageFrom }} - {{ course.languageTo }}</span
            >
          </div>
          <el-divider
            style="margin: 8px 0 !important"
            class="col-span-2"
            direction="horizontal"
          />
          <div class="cursor-pointer" @click="handleDescriptionDialog(course)">
            <div>
              <span>Description:</span>
            </div>
            <span class="font-bold">{{
              cutDescription(course.description)
            }}</span>
          </div>
          <div class="mt-auto">
            <el-button
              class="min-w-full"
              type="success"
              plain
              @click="handleJoiningToCourse(course)"
            >
              Join course
            </el-button>
          </div>
        </el-card>
      </div>
    </div>
  </div>

  <el-dialog
    v-model="courseDescriptionDialog"
    :title="courseDescriptionDialogTitle"
    width="70%"
    center
  >
    <el-descriptions :column="1" :border="true" class="mt-2">
      <el-descriptions-item label-class-name="w-32" label="Course name:">{{
        focusedItem?.name
      }}</el-descriptions-item>
      <el-descriptions-item label-class-name="w-32" label="Languages: ">{{
        focusedItem?.languageFrom + " - " + focusedItem?.languageTo
      }}</el-descriptions-item>
      <el-descriptions-item label-class-name="w-32" label="Description: ">
        <p v-for="text in focusedItem?.description.split('\n')">
          {{ text }}
        </p>
      </el-descriptions-item>
    </el-descriptions>
    <template #footer>
      <span class="dialog-footer">
        <el-button plain @click="courseDescriptionDialog = false"
          >Cancel</el-button
        >
      </span>
    </template>
  </el-dialog>
</template>

<style scoped></style>
