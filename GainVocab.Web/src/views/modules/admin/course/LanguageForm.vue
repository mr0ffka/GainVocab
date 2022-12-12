<script setup lang="ts">
import AdminMenu from "@/components/admin/AdminMenu.vue";
import { ILanguageAddModel } from "@/services/admin/types";
import {
  ElButton,
  ElForm,
  ElFormItem,
  ElInput,
  ElMessage,
  FormInstance,
} from "element-plus";
import { reactive, ref } from "vue";
import router from "@/router";
import { addLanguage } from "@/services/admin/adminApi";
import Header from "@/components/common/Header.vue";

const formRef = ref<FormInstance>();

const addModel: ILanguageAddModel = reactive({
  name: "",
});

const rules = reactive({
  name: [{ required: true, message: "Language name is required" }],
});

const languageAdd = (entity: ILanguageAddModel) =>
  addLanguage(entity)
    .then(() => {
      ElMessage({
        showClose: true,
        message: "Language added",
        type: "success",
      });
      router.push({ name: "language-list" });
    })
    .catch((error: any) => {
      error.response.data.Errors.forEach(async (e: any) => {
        ElMessage({
          showClose: true,
          message: e.Title,
          type: "error",
        });
      });
    });

const submitForm = async (formEl: FormInstance | undefined) => {
  if (!formEl) return;
  await formEl.validate((valid, fields) => {
    if (valid) {
      languageAdd({
        name: addModel.name,
      });
    } else {
      console.log("error submit!", fields);
    }
  });
};
</script>

<template>
  <Header></Header>
  <div class="flex grow">
    <AdminMenu />
    <div class="grow flex flex-col p-2">
      <div class="flex">
        <span class="font-bold text-xl">Add new language</span>
      </div>
      <el-form
        class="my-2"
        label-position="left"
        label-width="auto"
        ref="formRef"
        :model="addModel"
        :rules="rules"
      >
        <el-form-item prop="name" label="Language name">
          <el-input
            v-model="addModel.name"
            placeholder="Language name"
            clearable
            size="large"
          />
        </el-form-item>
      </el-form>
      <div>
        <el-button
          size="large"
          @click="router.go(-1)"
          type="info"
          plain
          class="mr-2 text-center !ml-auto"
        >
          Go back
        </el-button>
        <el-button
          size="large"
          type="success"
          plain
          @click="submitForm(formRef)"
          class="text-center !ml-auto"
        >
          <span>Add new language</span>
        </el-button>
      </div>
    </div>
  </div>
</template>

<style scoped></style>
