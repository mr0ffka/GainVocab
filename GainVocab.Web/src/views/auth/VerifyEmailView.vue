<script setup lang="ts">
import { verifyEmailFn } from '@/services/authApi';
import { ElMessage } from 'element-plus';
import { onMounted, ref } from 'vue';
import { useRoute, useRouter } from 'vue-router';

const userId = ref('');
const verificationCode = ref('');
const router = useRouter()
const route = useRoute();

onMounted(async () => {
  userId.value = route.query.userid;
  verificationCode.value = route.query.code;

  if (userId.value != "" && userId.value && verificationCode.value != "" && verificationCode.value) {
    let result = await verifyEmailFn(userId.value, verificationCode.value);
    if (result.succeeded) {
      ElMessage({
        showClose: true,
        message: "Email confirmation successful!",
        type: "success"
      });
    }
    else {
      ElMessage({
        showClose: true,
        message: "An error occured during email confirmation! Try again later.",
        type: "error"
      });
    }
    router.push("login")
  }
})
</script>

<template>

</template>