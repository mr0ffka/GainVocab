<script setup lang="ts">
import { verifyEmailFn } from "@/services/auth/authApi";
import { ElMessage } from "element-plus";
import { onMounted, ref } from "vue";
import { useRoute, useRouter } from "vue-router";

const userId = ref("");
const token = ref("");
const router = useRouter();
const route = useRoute();

onMounted(async () => {
  userId.value =
    typeof route.query.userid === "string"
      ? route.query.userid
      : route.query.userid?.[0] ?? "";
  token.value =
    typeof route.query.token === "string"
      ? route.query.token
      : route.query.token?.[0] ?? "";

  if (userId.value != "" && userId.value && token.value != "" && token.value) {
    let result = await verifyEmailFn(userId.value, token.value);
    if (result.succeeded) {
      ElMessage({
        showClose: true,
        message: "Email confirmation successful!",
        type: "success",
      });
    } else {
      ElMessage({
        showClose: true,
        message: "An error occured during email confirmation! Try again later.",
        type: "error",
      });
    }
    router.push("login");
  }
});
</script>

<template></template>
