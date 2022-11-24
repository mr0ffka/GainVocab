<script setup lang="ts">
import { onMounted, reactive, ref } from "vue";
import router from "@/router";
import { getCurrUser, logoutUserFn } from "@/services/authApi";
import { useMutation, useQuery } from "@tanstack/vue-query";
import { ElMenu, ElMenuItem, ElMessage } from "element-plus";
import { queryClient } from "@/helpers/queryClient";
import { Expand, Fold, Lock, MoreFilled } from "@element-plus/icons-vue";
import { useAuthMenuStore } from "@/store/authMenuStore";
import { mapActions, storeToRefs } from "pinia";

const isLogged = useQuery(["authUser"], getCurrUser);
const authMenuStore = useAuthMenuStore();
const { isMenuCollapsed } = storeToRefs(authMenuStore);

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
  <!-- not authenticated -->
  <el-menu
    v-if="!isLogged.data.value?.isAuthenticated && !isLogged.isLoading.value"
    :default-active="$route.path"
    class="flex"
    mode="horizontal"
    :router="true"
    :ellipsis="true"
  >
    <el-menu-item
      class="!border-b-0 hover:!bg-transparent focus:!bg-transparent"
      :index="{ name: 'home' }"
    >
      <span class="text-black">GainVocab</span>
    </el-menu-item>
    <el-menu-item class="!ml-auto" :index="{ name: 'login' }">
      <span>Login</span>
    </el-menu-item>
    <el-menu-item :index="{ name: 'register' }">
      <span>Register</span>
    </el-menu-item>
  </el-menu>

  <!-- admin -->
  <el-menu
    v-if="
      isLogged.data.value?.isAuthenticated &&
      isLogged.data.value?.isAdmin &&
      !isLogged.isLoading.value
    "
    :default-active="$route.path"
    class="flex"
    mode="horizontal"
    :router="true"
    :ellipsis="true"
  >
    <el-menu-item
      class="!border-b-0 hover:!bg-transparent focus:!bg-transparent pt-5 pb-5"
      index=""
      @click="authMenuStore.changeMenuCollapsed()"
    >
      <el-icon class="no-inherit">
        <fold v-if="!isMenuCollapsed" />
        <expand v-else />
      </el-icon>
    </el-menu-item>
    <el-menu-item
      class="!border-b-0 hover:!bg-transparent focus:!bg-transparent absolute top-0 left-1/2 -translate-x-1/2"
      :index="{ name: 'admin-dashboard' }"
    >
      <span class="text-black font-extrabold text-xl stroke-zinc-300"
        >GainVocab</span
      >
    </el-menu-item>
    <el-menu-item class="!border-b-0 focus:!bg-transparent !ml-auto" index="">
      <el-dropdown size="large">
        <el-icon class="pt-5 pb-5">
          <more-filled />
        </el-icon>
        <template #dropdown>
          <el-dropdown-menu>
            <el-dropdown-item @click="logout">
              <el-icon>
                <Lock />
              </el-icon>
              Log out
            </el-dropdown-item>
          </el-dropdown-menu>
        </template>
      </el-dropdown>
    </el-menu-item>
  </el-menu>

  <!-- user -->
  <el-menu
    v-if="
      isLogged.data.value?.isAuthenticated &&
      !isLogged.data.value?.isAdmin &&
      !isLogged.isLoading.value
    "
    :default-active="$route.path"
    class="flex"
    mode="horizontal"
    :router="true"
    :ellipsis="true"
  >
    <el-menu-item
      class="!border-b-0 hover:!bg-transparent focus:!bg-transparent"
      :index="{ name: 'home' }"
    >
      <span class="text-black font-black">GainVocab</span>
    </el-menu-item>
    <el-menu-item
      class="!border-b-0 focus:!bg-transparent !ml-auto"
      index=""
      @click="logout"
    >
      <span>Logout</span>
    </el-menu-item>
  </el-menu>
</template>
