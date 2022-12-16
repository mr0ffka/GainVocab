<script setup lang="ts">
import AdminMenu from "@/components/admin/AdminMenu.vue";
import Header from "@/components/common/Header.vue";
import {
  getCourseCount,
  getCourseDataCount,
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
const supportNewTicketsCount = ref<number>(0);

const userCountGet = () => {
  getUserCount()
    .then((data) => {
      console.log(data);
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
      console.log(data);
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
      console.log(data);
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
      console.log(data);
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

const supportNewTicketsCountGet = () => {
  getSupportIssueCount()
    .then((data) => {
      console.log(data);
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
  supportNewTicketsCountGet();
});
</script>

<template>
  <Header></Header>
  <div class="flex grow">
    <AdminMenu />
    <div class="grow grid lg:grid-cols-4 md:grid-cols-2 grid-rows-4 gap-2 p-2">
      <el-card
        class="box-card lg:col-span-4 md:col-span-2"
        body-style="display: flex; flex-flow:column; height: 75%; justify-content: center;"
      >
        <template #header>
          <router-link :to="{ name: 'support-list' }">
            <div class="card-header text-center">
              <span class="font-bold">New tickets</span>
            </div>
          </router-link>
        </template>
        <div class="text-center">
          <span class="font-bold text-8xl text-red-500">{{
            supportNewTicketsCount
          }}</span>
        </div>
      </el-card>
      <el-card
        class="box-card"
        body-style="display: flex; flex-flow:column; height: 75%; justify-content: center;"
      >
        <template #header>
          <router-link :to="{ name: 'user-list' }">
            <div class="card-header text-center">
              <span class="font-bold">Users</span>
            </div>
          </router-link>
        </template>
        <div class="text-center">
          <span class="font-bold text-8xl">{{ userCount }}</span>
        </div>
      </el-card>
      <el-card
        class="box-card"
        body-style="display: flex; flex-flow:column; height: 75%; justify-content: center;"
      >
        <template #header>
          <router-link :to="{ name: 'language-list' }">
            <div class="card-header text-center">
              <span class="font-bold">Languages</span>
            </div>
          </router-link>
        </template>
        <div class="text-center">
          <span class="font-bold text-8xl">{{ languageCount }}</span>
        </div>
      </el-card>
      <el-card
        class="box-card"
        body-style="display: flex; flex-flow:column; height: 75%; justify-content: center;"
      >
        <template #header>
          <router-link :to="{ name: 'course-list' }">
            <div class="card-header text-center">
              <span class="font-bold">Courses</span>
            </div>
          </router-link>
        </template>
        <div class="text-center">
          <span class="font-bold text-8xl">{{ courseCount }}</span>
        </div>
      </el-card>
      <el-card
        class="box-card"
        body-style="display: flex; flex-flow:column; height: 75%; justify-content: center;"
      >
        <template #header>
          <router-link :to="{ name: 'data-list' }">
            <div class="card-header text-center">
              <span class="font-bold">Course data entries</span>
            </div>
          </router-link>
        </template>
        <div class="text-center">
          <span class="font-bold text-8xl">{{ courseDataCount }}</span>
        </div>
      </el-card>
    </div>
  </div>
</template>

<style scoped></style>
