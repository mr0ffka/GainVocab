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
import { router } from "@/router";

const authMenuStore = useAuthMenuStore();
const { isMenuCollapsed } = storeToRefs(authMenuStore);
</script>

<template>
  <el-menu
    class="el-menu-vertical shadow-md "
    :collapse="isMenuCollapsed"
    :collapse-transition="false"
    :router="true"
    :default-active="$route.path"
    active-text-color="000"
    :default-openeds="['sm-1']"
  >
    <el-menu-item
      :index="
        router.getRoutes().filter((x) => x.name == 'admin-dashboard')[0].path
      "
      :route="{ name: 'admin-dashboard' }"
    >
      <el-icon><operation /></el-icon>
      <span>Dashboard</span>
    </el-menu-item>
    <el-menu-item
      :index="router.getRoutes().filter((x) => x.name == 'user-list')[0].path"
      :route="{ name: 'user-list' }"
    >
      <el-icon><user /></el-icon>
      <span>Users</span>
    </el-menu-item>

    <el-sub-menu index="sm-1">
      <template #title>
        <el-icon><reading /></el-icon>
        <span>Languages</span>
      </template>
      <el-menu-item-group>
        <el-menu-item
          :index="
            router.getRoutes().filter((x) => x.name == 'language-list')[0].path
          "
          :route="{ name: 'language-list' }"
          >Language list</el-menu-item
        >
        <el-menu-item
          :index="
            router.getRoutes().filter((x) => x.name == 'language-import')[0]
              .path
          "
          :route="{ name: 'language-import' }"
          >Import data</el-menu-item
        >
      </el-menu-item-group>
    </el-sub-menu>
    <el-menu-item
      :index="
        router.getRoutes().filter((x) => x.name == 'support-list')[0].path
      "
      :route="{ name: 'support-list' }"
    >
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
