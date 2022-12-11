<script setup lang="ts"></script>

<template>
  <div class="max-w-4xl mx-auto flex justify-center items-center h-screen">
    <div class="text-center">
      <div class="text-5xl font-bold">503</div>
      <div class="block text-xl font-semibold">Service Unavailable</div>
      <el-button
        size="large"
        @click="$router.go(-1)"
        type="info"
        plain
        class="mt-10 text-center"
      >
        Go back
      </el-button>
    </div>
  </div>
</template>

<style scoped></style>
