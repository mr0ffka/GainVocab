<script setup lang="ts">
import UserMenu from "@/components/user/UserMenu.vue";
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

// const userCountGet = () => {
//   getUserCount()
//     .then((data) => {
//       userCount.value = data;
//     })
//     .catch((error) => {
//       error.response.data.Errors.forEach(async (e: any) => {
//         ElMessage({
//           showClose: true,
//           message: e.Title,
//           type: "error",
//         });
//       });
//     });
// };

onMounted(() => {
  // userCountGet();
  // languageCountGet();
  // courseCountGet();
  // courseDataCountGet();
  // courseDataExampleCountGet();
  // supportNewTicketsCountGet();
});
</script>

<template>
  <Header></Header>
  <div class="flex grow">
    <UserMenu />
    <div class="grow grid lg:grid-cols-4 md:grid-cols-2 grid-rows-4 gap-2 p-2">
      <!-- <el-card
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
          <div>
            <div>
              <span>Data</span>
            </div>
            <span class="font-bold text-6xl">{{ courseDataCount }}</span>
          </div>
          <div>
            <div>
              <span>Examples</span>
            </div>
            <span class="font-bold text-6xl">{{ courseDataExampleCount }}</span>
          </div>
        </div>
      </el-card> -->
    </div>
  </div>
</template>

<style scoped></style>
