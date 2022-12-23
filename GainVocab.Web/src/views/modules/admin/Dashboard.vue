<script setup lang="ts">
import AdminMenu from "@/components/admin/AdminMenu.vue";
import Header from "@/components/common/Header.vue";
import {
  getCourseCount,
  getCourseDataCount,
  getCourseDataExampleCount,
  getLanguageCount,
  getSupportIssueCount,
  getUserCount,
} from "@/services/admin/adminApi";
import { ElMessage } from "element-plus";
import { onMounted, ref } from "vue";

const userCount = ref<number>(0);
const languageCount = ref<number>(0);
const courseCount = ref<number>(0);
const courseDataCount = ref<number>(0);
const courseDataExampleCount = ref<number>(0);
const supportNewTicketsCount = ref<number>(0);

const userCountGet = () => {
  getUserCount()
    .then((data) => {
      userCount.value = data;
    })
    .catch((error) => {
      error.response.data.Errors.forEach(async (e: any) => {
        ElMessage({
          showClose: true,
          message: e.Title,
          type: "error",
        });
      });
    });
};

const languageCountGet = () => {
  getLanguageCount()
    .then((data) => {
      languageCount.value = data;
    })
    .catch((error) => {
      error.response.data.Errors.forEach(async (e: any) => {
        ElMessage({
          showClose: true,
          message: e.Title,
          type: "error",
        });
      });
    });
};

const courseCountGet = () => {
  getCourseCount()
    .then((data) => {
      courseCount.value = data;
    })
    .catch((error) => {
      error.response.data.Errors.forEach(async (e: any) => {
        ElMessage({
          showClose: true,
          message: e.Title,
          type: "error",
        });
      });
    });
};

const courseDataCountGet = () => {
  getCourseDataCount()
    .then((data) => {
      courseDataCount.value = data;
    })
    .catch((error) => {
      error.response.data.Errors.forEach(async (e: any) => {
        ElMessage({
          showClose: true,
          message: e.Title,
          type: "error",
        });
      });
    });
};

const courseDataExampleCountGet = () => {
  getCourseDataExampleCount()
    .then((data) => {
      courseDataExampleCount.value = data;
    })
    .catch((error) => {
      error.response.data.Errors.forEach(async (e: any) => {
        ElMessage({
          showClose: true,
          message: e.Title,
          type: "error",
        });
      });
    });
};

const supportNewTicketsCountGet = () => {
  getSupportIssueCount()
    .then((data) => {
      supportNewTicketsCount.value = data;
    })
    .catch((error) => {
      error.response.data.Errors.forEach(async (e: any) => {
        ElMessage({
          showClose: true,
          message: e.Title,
          type: "error",
        });
      });
    });
};

onMounted(() => {
  userCountGet();
  languageCountGet();
  courseCountGet();
  courseDataCountGet();
  courseDataExampleCountGet();
  supportNewTicketsCountGet();
});
</script>

<template>
  <Header></Header>
  <div class="flex grow">
    <AdminMenu />
    <div class="grow grid lg:grid-cols-4 md:grid-cols-2 grid-rows-2 gap-2 p-2">
      <el-card
        @click="
          $router.push({ name: 'support-list', query: { isResolved: 'false' } })
        "
        shadow="hover"
        class="box-card lg:col-span-4 md:col-span-2 cursor-pointer"
        body-style="display: flex; flex-flow:column; height: 75%; justify-content: center;"
      >
        <template #header>
          <div class="card-header text-center">
            <span class="font-bold">Unresolved issues</span>
          </div>
        </template>
        <div class="text-center">
          <span class="font-bold text-8xl text-red-500">{{
            supportNewTicketsCount
          }}</span>
        </div>
      </el-card>
      <el-card
        @click="$router.push({ name: 'user-list' })"
        shadow="hover"
        class="box-card cursor-pointer"
        body-style="display: flex; flex-flow:column; height: 75%; justify-content: center;"
      >
        <template #header>
          <div class="card-header text-center">
            <span class="font-bold">Users</span>
          </div>
        </template>
        <div class="text-center">
          <span class="font-bold text-8xl">{{ userCount }}</span>
        </div>
      </el-card>
      <el-card
        @click="$router.push({ name: 'language-list' })"
        shadow="hover"
        class="box-card cursor-pointer"
        body-style="display: flex; flex-flow:column; height: 75%; justify-content: center;"
      >
        <template #header>
          <div class="card-header text-center">
            <span class="font-bold">Languages</span>
          </div>
        </template>
        <div class="text-center">
          <span class="font-bold text-8xl">{{ languageCount }}</span>
        </div>
      </el-card>
      <el-card
        @click="$router.push({ name: 'course-list' })"
        shadow="hover"
        class="box-card cursor-pointer"
        body-style="display: flex; flex-flow:column; height: 75%; justify-content: center;"
      >
        <template #header>
          <div class="card-header text-center">
            <span class="font-bold">Courses</span>
          </div>
        </template>
        <div class="text-center">
          <span class="font-bold text-8xl">{{ courseCount }}</span>
        </div>
      </el-card>
      <el-card
        @click="$router.push({ name: 'data-list' })"
        shadow="hover"
        class="box-card cursor-pointer"
        body-style="display: flex; flex-flow:column; height: 75%; justify-content: center;"
      >
        <template #header>
          <div class="card-header text-center">
            <span class="font-bold">Course data entries</span>
          </div>
        </template>
        <div class="text-center">
          <div>
            <div>
              <span>Data</span>
            </div>
            <span class="font-bold text-6xl">{{ courseDataCount }}</span>
          </div>
          <el-divider
            style="margin: 8px 0 !important"
            class="col-span-2"
            direction="horizontal"
          />
          <div>
            <div>
              <span>Examples</span>
            </div>
            <span class="font-bold text-6xl">{{ courseDataExampleCount }}</span>
          </div>
        </div>
      </el-card>
    </div>
  </div>
</template>

<style scoped></style>
