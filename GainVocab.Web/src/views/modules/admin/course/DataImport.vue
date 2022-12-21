<script setup lang="ts">
import AdminMenu from "@/components/admin/AdminMenu.vue";
import Header from "@/components/common/Header.vue";
import router from "@/router";
import {
  applyImportData,
  deleteImportFiles,
  getCoursesOptionsList,
} from "@/services/admin/adminApi";
import { ICourseItemModel } from "@/services/admin/types";
import { UploadFilled } from "@element-plus/icons-vue";
import {
  ElMessage,
  UploadFile,
  UploadFiles,
  UploadInstance,
} from "element-plus";
import { onMounted, ref } from "vue";

const withExamples = ref<boolean>(false);
const uploadSucceeded = ref<boolean>(false);
const coursesOptions = ref<ICourseItemModel[] | null>();
const uploadRef = ref<UploadInstance>();
const url = import.meta.env.VITE_API_URL;
const selectedCourseId = ref<string>("");

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

onMounted(() => {
  getCoursesOptions();
});

const onSuccessUpload = (
  response: any,
  uploadFile: UploadFile,
  uploadFiles: UploadFiles
) => {
  ElMessage({
    showClose: true,
    message: "uploaded " + uploadFile.name,
    type: "success",
  });
  uploadSucceeded.value = true;
};

const onErrorUpload = () => {
  ElMessage({
    showClose: true,
    message: "error during upload",
    type: "error",
  });
};

const deleteEntity = async (id: string) =>
  await deleteImportFiles(id)
    .then((data: any) => {
      ElMessage({
        showClose: true,
        message: "Files has been deleted",
        type: "success",
      });
    })
    .catch((error: any) => {
      ElMessage({
        showClose: true,
        message: error.response.data.Title,
        type: "error",
      });
    });

const applyImport = async (id: string) =>
  await applyImportData(id)
    .then((data: any) => {
      ElMessage({
        showClose: true,
        message: "Data has been applied",
        type: "success",
      });
      router.push({
        name: "data-list",
        query: { coursePublicId: selectedCourseId.value },
      });
    })
    .catch((error: any) => {
      ElMessage({
        showClose: true,
        message: error.response.data.Title,
        type: "error",
      });
    });

const submitUpload = () => {
  uploadRef.value!.submit();
};

const applyChanges = () => {
  applyImport(selectedCourseId.value);
};

const deleteFiles = () => {
  deleteEntity(selectedCourseId.value);
  withExamples.value = false;
  uploadSucceeded.value = false;
  uploadRef.value?.clearFiles();
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
      <el-card class="box-card mt-2" shadow="never">
        <span class="inline-block align-middle font-bold">Select course:</span>
        <el-select
          v-model="selectedCourseId"
          placeholder="Select"
          class="ml-2"
          :disabled="uploadSucceeded"
        >
          <el-option
            v-for="item in coursesOptions"
            :key="item.id"
            :label="item.name"
            :value="item.id"
          />
        </el-select>
        <el-checkbox
          v-if="selectedCourseId != ''"
          class="ml-2"
          :disabled="uploadSucceeded"
          v-model="withExamples"
          label="With examples"
          size="large"
        />
        <div class="mt-2" v-if="selectedCourseId != ''">
          <el-upload
            class="upload-demo"
            drag
            :action="
              url + 'course-data/import?coursePublicId=' + selectedCourseId
            "
            :multiple="withExamples"
            :auto-upload="false"
            :limit="withExamples ? 2 : 1"
            :show-file-list="true"
            :with-credentials="true"
            :on-success="onSuccessUpload"
            :on-error="onErrorUpload"
            ref="uploadRef"
            :disabled="uploadSucceeded"
          >
            <el-icon class="el-icon--upload"><upload-filled /></el-icon>
            <div class="el-upload__text">
              Drop file here or <em>click to upload</em>
            </div>
            <template #tip>
              <div class="el-upload__tip">
                <span v-if="!withExamples">
                  csv file with words/phrases translations.
                </span>
                <span v-else>
                  csv files with words/phrases translations and example
                  sentenses.
                </span>
              </div>
            </template>
          </el-upload>
          <div class="mt-2">
            <el-button v-if="!uploadSucceeded" plain @click="submitUpload">
              Upload files to server
            </el-button>
            <div v-if="uploadSucceeded">
              <el-button type="success" plain @click="applyChanges">
                Apply changes
              </el-button>
              <el-button type="danger" plain @click="deleteFiles">
                Delete files
              </el-button>
            </div>
          </div>
        </div>
      </el-card>
    </div>
  </div>
</template>

<style scoped></style>
