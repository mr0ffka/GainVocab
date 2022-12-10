<script setup lang="ts"></script>

<template>
  <div class="common-layout">
    <el-container>
      <el-main>
        <section class="bg-ct-blue-600 pt-20">
          <div
            class="max-w-4xl mx-auto bg-ct-dark-200 rounded-md h-[20rem] flex justify-center items-center"
          >
            <div>
              <span class="text-5xl font-bold">Gain Vocab</span>
              <span class="block text-xl font-semibold"
                >Learn languages in fun way!</span
              >
            </div>
          </div>
          <el-row class="flex justify-center">
            <router-link class="" :to="{ name: 'login' }">
              <el-button class="w-40"> Login </el-button>
            </router-link>
            <router-link class="" :to="{ name: 'register' }">
              <el-button class="w-40 ml-2" type="primary" plain>
                Join now
              </el-button>
            </router-link>
          </el-row>
        </section>
      </el-main>
    </el-container>
  </div>
</template>

<style scoped></style>
