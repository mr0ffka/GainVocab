<script setup lang="ts">
import { onMounted, ref } from "vue";
import {
  Help,
  Menu as IconMenu,
  User,
  Operation,
  Reading,
} from "@element-plus/icons-vue";
import { useAuthMenuStore } from "@/store/authMenuStore";
import { storeToRefs } from "pinia";

const authMenuStore = useAuthMenuStore();
const { isMenuCollapsed } = storeToRefs(authMenuStore);
</script>

<template>
  <el-menu
    class="el-menu-vertical"
    :collapse="isMenuCollapsed"
    :collapse-transition="false"
    :router="true"
    :default-active="{ name: 'admin-dashboard' }"
  >
    <el-menu-item :index="{ name: 'admin-dashboard' }">
      <el-icon><operation /></el-icon>
      <span>Dashboard</span>
    </el-menu-item>
    <el-menu-item :index="{ name: 'users' }">
      <el-icon><user /></el-icon>
      <span>Users</span>
    </el-menu-item>
    <el-sub-menu :router="true">
      <template #title>
        <el-icon><reading /></el-icon>
        <span>Languages</span>
      </template>
      <el-menu-item-group>
        <el-menu-item :index="{ name: 'languages' }"
          >Language list</el-menu-item
        >
        <el-menu-item :index="{ name: 'language-import' }"
          >Import data</el-menu-item
        >
      </el-menu-item-group>
    </el-sub-menu>
    <el-menu-item :index="{ name: 'support' }">
      <el-icon><help /></el-icon>
      <span>Support</span>
    </el-menu-item>
  </el-menu>
</template>

<style scoped>
.el-menu-vertical:not(.el-menu--collapse) {
  min-width: 200px;
  min-height: 400px;
}
</style>
