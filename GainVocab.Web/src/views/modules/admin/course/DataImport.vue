<script setup lang="ts">
import AdminMenu from "@/components/admin/AdminMenu.vue";
import Header from "@/components/common/Header.vue";
import { getCoursesOptionsList } from "@/services/admin/adminApi";
import { ICourseItemModel } from "@/services/admin/types";
import { UploadFilled } from "@element-plus/icons-vue";
import { ElMessage } from "element-plus";
import { ref } from "vue";

const withExamples = ref<boolean>(false);
const coursesOptions = ref<ICourseItemModel[] | null>();

const getCoursesOptions = async () => {
  await getCoursesOptionsList()
    .then((data: ICourseItemModel[]) => {
      let options: ICourseItemModel[] = [];
      data.forEach((el) => {
        options.push(el);
      });
      coursesOptions.value = options;
    })
    .catch((error) => {
      error.response.data.Errors.forEach(async (e: any) => {
        ElMessage({
          showClose: true,
          message: e.Title,
          type: "error",
        });
      });
    });
};
</script>

<template>
  <Header></Header>
  <div class="flex grow">
    <AdminMenu />
    <div class="grow flex flex-col p-2">
      <div class="flex">
        <span class="font-bold text-xl">Upload data</span>
      </div>
      <div>
        <span>Upload words/phrases with or without examples</span>
      </div>
      <div>
        <el-checkbox
          v-model="withExamples"
          label="With examples"
          size="large"
        />
      </div>
      <el-card class="box-card" shadow="never">
        <el-upload
          class="upload-demo"
          drag
          action="https://run.mocky.io/v3/9d059bf9-4660-45f2-925d-ce80ad6c4d15"
          :multiple="false"
        >
          <el-icon class="el-icon--upload"><upload-filled /></el-icon>
          <div class="el-upload__text">
            Drop file here or <em>click to upload</em>
          </div>
          <template #tip>
            <div class="el-upload__tip">
              csv file with words/phrases translations.
            </div>
          </template>
        </el-upload>
      </el-card>

      <el-card v-if="withExamples" class="box-card my-2" shadow="never">
        <el-upload
          class="upload-demo"
          drag
          action="https://run.mocky.io/v3/9d059bf9-4660-45f2-925d-ce80ad6c4d15"
          :multiple="false"
        >
          <el-icon class="el-icon--upload"><upload-filled /></el-icon>
          <div class="el-upload__text">
            Drop file here or <em>click to upload</em>
          </div>
          <template #tip>
            <div class="el-upload__tip">
              csv file with example sentenses using words/pharses from above
              file.
            </div>
          </template>
        </el-upload>
      </el-card>
    </div>
  </div>
</template>

<style scoped></style>
