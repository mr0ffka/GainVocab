<script setup lang="ts">
import { onMounted, ref } from "vue";
import {
  Document,
  Menu as IconMenu,
  Location,
  Setting,
} from "@element-plus/icons-vue";
import { useAuthMenuStore } from "@/store/authMenuStore";
import { storeToRefs } from "pinia";

const authMenuStore = useAuthMenuStore();
const { isMenuCollapsed } = storeToRefs(authMenuStore);

// onMounted(() => {
// //   isExpended.value = true;
// //   window.addEventListener("adminMenuExpended-localstorage-changed", (event) => {
// //     //console.log(event);
// //     isExpended.value = Boolean(JSON.parse(event.detail.storage));
// //   });
// // });
</script>

<template>
  <div class="flex grow">
    <el-menu
      default-active="2"
      class="el-menu-vertical"
      :collapse="isMenuCollapsed"
      :collapse-transition="true"
    >
      <el-sub-menu index="1">
        <template #title>
          <el-icon><location /></el-icon>
          <span>Navigator One</span>
        </template>
        <el-menu-item-group>
          <template #title><span>Group One</span></template>
          <el-menu-item index="1-1">item one</el-menu-item>
          <el-menu-item index="1-2">item two</el-menu-item>
        </el-menu-item-group>
        <el-menu-item-group title="Group Two">
          <el-menu-item index="1-3">item three</el-menu-item>
        </el-menu-item-group>
      </el-sub-menu>
      <el-menu-item index="2">
        <el-icon><icon-menu /></el-icon>
        <template #title>Navigator Two</template>
      </el-menu-item>
      <el-menu-item index="3" disabled>
        <el-icon><document /></el-icon>
        <template #title>Navigator Three</template>
      </el-menu-item>
      <el-menu-item index="4">
        <el-icon><setting /></el-icon>
        <template #title>Navigator Four</template>
      </el-menu-item>
    </el-menu>
  </div>
</template>

<style scoped>
.el-menu-vertical:not(.el-menu--collapse) {
  min-width: 200px;
  min-height: 400px;
}
</style>
