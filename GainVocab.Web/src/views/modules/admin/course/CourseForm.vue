<script setup lang="ts">
import AdminMenu from "@/components/admin/AdminMenu.vue";
import { ICourseAddModel, ILanguageModel } from "@/services/admin/types";
import {
  ElButton,
  ElForm,
  ElFormItem,
  ElInput,
  ElMessage,
  FormInstance,
} from "element-plus";
import { onMounted, reactive, ref } from "vue";
import router from "@/router";
import { addCourse, getLanguageOptionsList } from "@/services/admin/adminApi";
import Header from "@/components/common/Header.vue";

const formRef = ref<FormInstance>();
const languageOptions = ref<ILanguageModel[] | null>();

const addModel: ICourseAddModel = reactive({
  name: "",
  languageFrom: "",
  languageTo: "",
  description: "",
});

const rules = reactive({
  name: [{ required: true, message: "Course name is required" }],
  languageFrom: [{ required: true, message: "Select language from!" }],
  languageTo: [{ required: true, message: "Select language to!" }],
});

const courseAdd = (entity: ICourseAddModel) =>
  addCourse(entity)
    .then(() => {
      ElMessage({
        showClose: true,
        message: "Course added",
        type: "success",
      });
      router.push({ name: "course-list" });
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

const getLanguageOptions = async () => {
  await getLanguageOptionsList()
    .then((data: ILanguageModel[]) => {
      let options: ILanguageModel[] = [];
      data.forEach((el) => {
        options.push(el);
      });
      languageOptions.value = options;
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

const submitForm = async (formEl: FormInstance | undefined) => {
  if (!formEl) return;
  await formEl.validate((valid, fields) => {
    if (valid) {
      courseAdd({
        name: addModel.name,
        languageFrom: addModel.languageFrom,
        languageTo: addModel.languageTo,
        description: addModel.description,
      });
    } else {
      console.log("error submit!", fields);
    }
  });
};

onMounted(() => {
  getLanguageOptions();
});
</script>

<template>
  <Header></Header>
  <div class="flex grow">
    <AdminMenu />
    <div class="grow flex flex-col p-2">
      <div class="flex">
        <span class="font-bold text-xl">Add new course</span>
      </div>
      <el-form
        class="my-2"
        label-position="left"
        label-width="auto"
        ref="formRef"
        :model="addModel"
        :rules="rules"
      >
        <el-form-item prop="name" label="Course name">
          <el-input
            v-model="addModel.name"
            placeholder="Course name"
            clearable
            size="large"
          />
        </el-form-item>
        <el-form-item class="text-lg" prop="languageFrom" label="Language from">
          <el-select
            v-model="addModel.languageFrom"
            placeholder="Language from"
            size="large"
            clearable
            class="grow"
          >
            <el-option
              v-for="item in languageOptions"
              :key="item.id"
              :label="item.name"
              :value="item.id"
            />
          </el-select>
        </el-form-item>
        <el-form-item class="text-lg" prop="languageTo" label="Language to">
          <el-select
            v-model="addModel.languageTo"
            placeholder="Language to"
            size="large"
            clearable
            class="grow"
          >
            <el-option
              v-for="item in languageOptions"
              :key="item.id"
              :label="item.name"
              :value="item.id"
            />
          </el-select>
        </el-form-item>
        <el-form-item
          class="text-lg"
          prop="description"
          label="Course Description"
        >
          <el-input
            v-model="addModel.description"
            placeholder="Course description"
            size="large"
            class="grow"
            type="textarea"
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
          <span>Add new course</span>
        </el-button>
      </div>
    </div>
  </div>
</template>

<style scoped></style>
