<script setup lang="ts">
import { onMounted, reactive, ref } from "vue";
import router from "@/router";
import { getCurrUser, logoutUserFn } from "@/services/authApi";
import { useMutation, useQuery } from "@tanstack/vue-query";
import { ElMenu, ElMenuItem, ElMessage } from "element-plus";
import { queryClient } from "@/helpers/queryClient";

const isLogged = useQuery(["authUser"], getCurrUser);

const handleSelect = (key: string, keyPath: string[]) => {
  console.log(key, keyPath);
};

onMounted(() => {
  console.log(router);
});

const mutation = useMutation(() => logoutUserFn(), {
  onError: (error: any) => {
    ElMessage({
      showClose: true,
      message: (error as any).response.data.message,
      type: "error",
    });
  },
  onSuccess: async (data: any) => {
    ElMessage({
      showClose: true,
      message: "Successfully logged out",
      type: "success",
    });
    queryClient.setQueriesData(["authUser"], {});
    await router.push({ name: "login" });
  },
});

const logout = () => {
  mutation.mutate();
};
</script>

<template>
  <el-menu
    :default-active="$route.path"
    class="flex grow"
    mode="horizontal"
    :router="true"
    :ellipsis="true"
    @select="handleSelect"
  >
    <el-menu-item
      class="!border-b-0 hover:!bg-transparent focus:!bg-transparent"
      :index="{ name: 'home' }"
    >
      <span class="text-black">GainVocab</span>
    </el-menu-item>
    <el-menu-item
      class="!ml-auto"
      :index="{ name: 'login' }"
      v-if="!isLogged.data.value?.isAuthenticated && !isLogged.isLoading.value"
    >
      <span>Login</span>
    </el-menu-item>
    <el-menu-item
      v-if="isLogged.data.value?.isAuthenticated && !isLogged.isLoading.value"
      class="!border-b-0 focus:!bg-transparent !ml-auto"
      index=""
      @click="logout"
    >
      <span>Logout</span>
    </el-menu-item>
    <el-menu-item
      :index="{ name: 'register' }"
      v-if="!isLogged.data.value?.isAuthenticated && !isLogged.isLoading.value"
    >
      <span>Register</span>
    </el-menu-item>
  </el-menu>
</template>
