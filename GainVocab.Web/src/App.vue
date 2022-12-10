<script setup lang="ts">
import Header from "@/components/common/Header.vue";
import { useQuery } from "@tanstack/vue-query";
import { getCurrUser } from "./services/auth/authApi";

useQuery(["authUser"], getCurrUser);
</script>

<template>
  <router-view :key="$route.fullPath"></router-view>
</template>

<style scoped></style>
