<script setup lang="ts">
import AdminMenu from "@/components/admin/AdminMenu.vue";
import {
  ICourseDataAddModel,
  ICourseDataUpdateModel,
  ICourseItemModel,
} from "@/services/admin/types";
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
import {
  addCourseData,
  getCourseData,
  getCoursesOptionsList,
  updateCourseData,
} from "@/services/admin/adminApi";
import Header from "@/components/common/Header.vue";
import { useRoute } from "vue-router";

const route = useRoute();
const formRef = ref<FormInstance>();
const entityId = ref<string>("");
const coursesOptions = ref<ICourseItemModel[] | null>();

const addModel: ICourseDataAddModel = reactive({
  source: "",
  translation: "",
  coursePublicId: "",
});

const rules = reactive({
  source: [{ required: true, message: "Source text is required" }],
  translation: [{ required: true, message: "Translation text is required" }],
  coursePublicId: [
    {
      required: true,
      message: "Choose course! Course is required!",
      trigger: "change",
    },
  ],
});

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

const courseDataAdd = (entity: ICourseDataAddModel) =>
  addCourseData(entity)
    .then(() => {
      ElMessage({
        showClose: true,
        message: "Data added",
        type: "success",
      });
      router.push({ name: "data-list" });
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

const courseDataUpdate = (entity: ICourseDataUpdateModel) =>
  updateCourseData(entityId.value, entity)
    .then(() => {
      ElMessage({
        showClose: true,
        message: "Data updated",
        type: "success",
      });
      router.push({ name: "data-list" });
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

const courseDataGet = () =>
  getCourseData(entityId.value.toString())
    .then((data: ICourseDataAddModel) => {
      addModel.coursePublicId = data.coursePublicId;
      addModel.source = data.source;
      addModel.translation = data.translation;
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
      if (entityId.value === "" || entityId.value === null) {
        courseDataAdd({
          source: addModel.source,
          translation: addModel.translation,
          coursePublicId: addModel.coursePublicId,
        });
      } else {
        courseDataUpdate({
          source: addModel.source,
          translation: addModel.translation,
        });
      }
    } else {
      console.log("error submit!", fields);
    }
  });
};

onMounted(() => {
  if (route.params.publicId !== undefined) {
    entityId.value = route.params.publicId.toString();
    courseDataGet();
  }
  getCoursesOptions();
});
</script>

<template>
  <Header></Header>
  <div class="flex grow">
    <AdminMenu />
    <div class="grow flex flex-col p-2">
      <div class="flex">
        <span class="font-bold text-xl" v-if="entityId !== ''"
          >Update course data</span
        >
        <span class="font-bold text-xl" v-else>Add new course data</span>
      </div>
      <el-form
        class="my-2"
        label-position="left"
        label-width="auto"
        ref="formRef"
        :model="addModel"
        :rules="rules"
      >
        <el-form-item prop="coursePublicId" label="Course">
          <el-select
            v-model="addModel.coursePublicId"
            placeholder="Select course"
            size="large"
            class="grow"
            :disabled="entityId !== ''"
          >
            <el-option
              v-for="item in coursesOptions"
              :key="item.id"
              :label="item.name"
              :value="item.id"
            />
          </el-select>
        </el-form-item>
        <el-form-item prop="source" label="Source text">
          <el-input
            v-model="addModel.source"
            placeholder="Source text"
            clearable
            size="large"
          />
        </el-form-item>
        <el-form-item prop="translation" label="Translation text">
          <el-input
            v-model="addModel.translation"
            placeholder="Translation text"
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
          <span v-if="entityId !== ''">Update course data</span>
          <span v-else>Add course data</span>
        </el-button>
      </div>
    </div>
  </div>
</template>

<style scoped></style>
