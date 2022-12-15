<script setup lang="ts">
import {
  Help,
  User,
  Operation,
  Reading,
  Upload,
  Notebook,
  Connection,
  House,
} from "@element-plus/icons-vue";
import { useAuthMenuStore } from "@/store/authMenuStore";
import { storeToRefs } from "pinia";
import { router } from "@/router";

const authMenuStore = useAuthMenuStore();
const { isMenuCollapsed } = storeToRefs(authMenuStore);
</script>

<template>
  <el-menu
    class="el-menu-vertical shadow-md"
    :collapse="isMenuCollapsed"
    :collapse-transition="false"
    :router="true"
    active-text-color="black"
    :default-active="$route.path"
    :default-openeds="['sm-1', 'sm-2']"
  >
    <el-menu-item
      :index="
        router.getRoutes().filter((x) => x.name == 'admin-dashboard')[0].path
      "
      :route="{ name: 'admin-dashboard' }"
    >
      <!-- <el-icon><operation /></el-icon> -->
      <el-icon><House /></el-icon>
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
        <span>Courses</span>
      </template>
      <el-menu-item-group>
        <el-menu-item
          :index="
            router.getRoutes().filter((x) => x.name == 'language-list')[0].path
          "
          :route="{ name: 'language-list' }"
        >
          <el-icon><Connection /></el-icon>
          <span>Languages</span>
        </el-menu-item>
        <el-menu-item
          :index="
            router.getRoutes().filter((x) => x.name == 'course-list')[0].path
          "
          :route="{ name: 'course-list' }"
        >
          <el-icon><reading /></el-icon>
          <span>Courses</span>
        </el-menu-item>
        <el-sub-menu index="sm-2">
          <template #title>
            <el-icon><Notebook /></el-icon>
            <span>Course data</span>
          </template>
          <el-menu-item
            :index="
              router.getRoutes().filter((x) => x.name == 'data-list')[0].path
            "
            :route="{ name: 'data-list' }"
          >
            <el-icon><Notebook /></el-icon>
            <span>Data</span></el-menu-item
          >
          <el-menu-item
            :index="
              router.getRoutes().filter((x) => x.name == 'data-import')[0].path
            "
            :route="{ name: 'data-import' }"
          >
            <el-icon><Upload /></el-icon>
            <span>Import data</span>
          </el-menu-item>
        </el-sub-menu>
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
.is-active > .el-sub-menu__title > span,
.el-menu-item.is-active {
  font-weight: bold;
}
.el-menu-vertical:not(.el-menu--collapse) {
  min-width: 200px;
  min-height: 400px;
}
</style>
