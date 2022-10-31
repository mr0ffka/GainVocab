<script setup lang="ts">
import { ref } from "vue";
import router from "@/router";
import { logoutUserFn } from "@/services/authApi";
import { useMutation } from "@tanstack/vue-query";
import { ElMenu, ElMenuItem, ElMessage } from "element-plus";
import { queryClient } from "@/helpers/queryClient";
import { RouterLink } from "vue-router";

const activeIndex = ref("1");
const handleSelect = (key: string, keyPath: string[]) => {
  console.log(key, keyPath);
};

const mutation = useMutation(
  () => logoutUserFn(),
  {
    onError: (error: any) => {
      ElMessage({
        showClose: true,
        message: (error as any).response.data.message,
        type: "error"
      });
    },
    onSuccess: (data: any) => {
      ElMessage({
        showClose: true,
        message: "Successfully logged out",
        type: "success"
      });
      queryClient.removeQueries(['authUser'], { exact: true });
      router.push({ name: 'login' });
    },
  }
);

const logout = () => {
  mutation.mutate();
};
</script>

<template>
  <el-menu :default-active="$route.path" class="flex" mode="horizontal" :router="true" :ellipsis="true"
    @select="handleSelect">
    <el-menu-item class="!border-b-0 hover:!bg-transparent focus:!bg-transparent">
      <router-link :to="{ name: 'home' }"></router-link><span class="text-black">GainVocab</span>
    </el-menu-item>
    <el-menu-item class="!ml-auto">
      <router-link :to="{ name: 'login' }">Login</router-link>
    </el-menu-item>
    <el-menu-item class="!m1-auto">
      <a @click="logout">Logout</a>
    </el-menu-item>
    <el-menu-item>
      <router-link :to="{ name: 'register' }">Register</router-link>
    </el-menu-item>

    <!-- <el-sub-menu index="2">
      <template #title>Workspace</template>
      <el-menu-item index="2-1">item one</el-menu-item>
      <el-menu-item index="2-2">item two</el-menu-item>
      <el-menu-item index="2-3">item three</el-menu-item>
      <el-sub-menu index="2-4">
        <template #title>item four</template>
        <el-menu-item index="2-4-1">item one</el-menu-item>
        <el-menu-item index="2-4-2">item two</el-menu-item>
        <el-menu-item index="2-4-3">item three</el-menu-item>
      </el-sub-menu>
    </el-sub-menu> -->
  </el-menu>
</template>