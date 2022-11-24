<script setup lang="ts">
import Menu from "@/components/Header.vue";
import { useQuery } from "@tanstack/vue-query";
import { getCurrUser } from "./services/authApi";
const isLogged = useQuery(["authUser"], getCurrUser);
</script>

<template>
  <Menu></Menu>
  <router-view></router-view>
</template>

<style scoped></style>
