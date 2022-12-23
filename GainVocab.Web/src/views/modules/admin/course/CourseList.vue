<script setup lang="ts">
import AdminMenu from "@/components/admin/AdminMenu.vue";
import {
  editCourseDescription,
  getLanguageOptionsList,
  getListCourse,
  removeCourse,
} from "@/services/admin/adminApi";
import { storeToRefs } from "pinia";
import { onMounted, reactive, ref } from "vue";
import {
  ICourseDescriptionEditModel,
  ICourseListModel,
  ILanguageModel,
} from "@/services/admin/types";
import { IPagedResult, IPager } from "@/services/common/types";
import router from "@/router";
import { Plus, RefreshRight, Search } from "@element-plus/icons-vue";
import { ElMessage, FormInstance } from "element-plus";
import Header from "@/components/common/Header.vue";
import { useAdminCourseStore } from "@/store/admin/adminCourseStore";
import { useRoute } from "vue-router";

const store = useAdminCourseStore();
const { filter, pager, isSearching } = storeToRefs(store);
const confirmDeleteDialog = ref(false);
const editDescriptionDialog = ref(false);
const focusedItem = ref<ICourseListModel | null>();
let entities = ref<ICourseListModel[]>();
let pagerValues = ref<IPager>();
const languageOptions = ref<ILanguageModel[] | null>();

const formEditCourseDescriptionRef = ref<FormInstance>();
const courseDescriptionModel: ICourseDescriptionEditModel = reactive({
  description: "",
});

const getEntities = () => {
  isSearching.value = true;
  getListCourse(filter.value, pager.value)
    .then((data: IPagedResult) => {
      isSearching.value = false;
      entities.value = data.items;
      pagerValues.value = {
        pageNumber: data.pageNumber,
        totalCount: data.totalCount,
        recordNumber: data.recordNumber,
      };
    })
    .catch((error) => {
      isSearching.value = false;
      error.response.data.Errors.forEach(async (e: any) => {
        ElMessage({
          showClose: true,
          message: e.Title,
          type: "error",
        });
      });
    });
};

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
      isSearching.value = false;
      error.response.data.Errors.forEach(async (e: any) => {
        ElMessage({
          showClose: true,
          message: e.Title,
          type: "error",
        });
      });
    });
};

const deleteEntity = async (id: string) =>
  await removeCourse(id)
    .then((data: any) => {
      ElMessage({
        showClose: true,
        message: "Course with all the data has been deleted",
        type: "success",
      });
      getEntities();
    })
    .catch((error: any) => {
      ElMessage({
        showClose: true,
        message: error.response.data.Title,
        type: "error",
      });
    });

const editDescription = async (id: string) =>
  await editCourseDescription(id, courseDescriptionModel.description)
    .then((data: any) => {
      ElMessage({
        showClose: true,
        message: "Course description has been updated",
        type: "success",
      });
      getEntities();
    })
    .catch((error: any) => {
      ElMessage({
        showClose: true,
        message: error.response.data.Title,
        type: "error",
      });
    });

onMounted(() => {
  getEntities();
  getLanguageOptions();
});

const handleDeleteDialog = (row: ICourseListModel) => {
  focusedItem.value = row;
  confirmDeleteDialog.value = true;
};

const handleDelete = () => {
  deleteEntity(focusedItem.value?.id ?? "");
  confirmDeleteDialog.value = false;
  focusedItem.value = null;
};

const resetFilters = () => {
  store.resetFilters();
  getEntities();
};

const handleEditDescriptionDialog = (row: ICourseListModel) => {
  focusedItem.value = row;
  courseDescriptionModel.description = row.description;
  editDescriptionDialog.value = true;
};

const submitCourseDescriptionEdit = async (
  formEl: FormInstance | undefined
) => {
  if (!formEl) return;
  await formEl.validate((valid, fields) => {
    if (valid) {
      editDescription(focusedItem.value?.id ?? "");
      editDescriptionDialog.value = false;
      focusedItem.value = null;
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
        <div class="flex flex-row">
          <el-input
            v-model="filter.name"
            class="mb-2"
            placeholder="Course name"
          />
          <el-select
            v-model="filter.languageFrom"
            multiple
            collapse-tags
            collapse-tags-tooltip
            placeholder="Language from"
            clearable
            @clear="getEntities"
            class="ml-2 min-w-fit"
          >
            <el-option
              v-for="item in languageOptions"
              :key="item.id"
              :label="item.name"
              :value="item.id"
            />
          </el-select>
          <el-select
            v-model="filter.languageTo"
            multiple
            collapse-tags
            collapse-tags-tooltip
            placeholder="Language to"
            clearable
            @clear="getEntities"
            class="ml-2 min-w-fit"
          >
            <el-option
              v-for="item in languageOptions"
              :key="item.id"
              :label="item.name"
              :value="item.id"
            />
          </el-select>
        </div>
        <div class="flex flex-row !ml-auto">
          <el-button
            class="mb-2 mr-2 p-3 font-bold right !ml-auto"
            plain
            :loading="isSearching"
            @click="getEntities"
            ><el-icon><Search /></el-icon>&nbsp;Search</el-button
          >
          <el-button
            class="mb-2 mr-2 p-3 font-bold right !ml-auto"
            plain
            type="info"
            @click="resetFilters"
            ><el-icon><RefreshRight /></el-icon>&nbsp;Reset filters</el-button
          >
          <el-button
            class="mb-2 p-3 font-bold right !ml-auto"
            type="success"
            plain
            @click="router.push({ name: 'course-add' })"
            ><el-icon><Plus /></el-icon>&nbsp;Add course</el-button
          >
        </div>
      </div>
      <el-table
        :data="entities ?? []"
        :default-sort="{ prop: 'name', order: 'descending' }"
        :flexible="true"
        :border="true"
        :stripe="true"
      >
        <el-table-column type="expand">
          <template #default="scope">
            <div class="mx-2">
              <el-descriptions
                v-if="scope.row.description != ''"
                :column="1"
                :border="true"
                class="mt-2"
              >
                <el-descriptions-item label="Course description">
                  <p v-for="text in scope.row.description.split('\n')">
                    {{ text }}
                  </p>
                </el-descriptions-item>
              </el-descriptions>
              <div class="mt-2 flex flex-row">
                <el-button
                  type="info"
                  plain
                  @click="handleEditDescriptionDialog(scope.row)"
                >
                  <span v-if="scope.row.description != ''"
                    >Edit description</span
                  >
                  <span v-else>Add description</span>
                </el-button>
              </div>
            </div>
          </template>
        </el-table-column>
        <el-table-column
          label-class-name="font-black"
          prop="name"
          label="Course"
          sortable
          width=""
        />
        <el-table-column
          label-class-name="font-black"
          prop="languageFrom"
          label="Language From"
          sortable
          width=""
        >
        </el-table-column>
        <el-table-column
          label-class-name="font-black"
          prop="languageTo"
          label="Language To"
          sortable
          width=""
        >
        </el-table-column>
        <el-table-column
          label-class-name="font-black"
          fixed="right"
          label="Operations"
          width=""
          type="not-clickable"
        >
          <template #default="scope">
            <div class="flex flex-row">
              <el-button
                size="small"
                type="danger"
                plain
                @click="handleDeleteDialog(scope.row)"
                >Delete</el-button
              >
            </div>
          </template>
        </el-table-column>
      </el-table>
      <div class="relative my-2">
        <el-pagination
          class=""
          v-model:page-size="pager.pageSize"
          :page-sizes="[5, 10, 15, 50]"
          :small="false"
          :disabled="false"
          :background="true"
          layout="->,total,sizes"
          :total="pagerValues?.totalCount ?? 0"
          @current-change="getEntities()"
          @size-change="getEntities()"
        />
        <el-pagination
          class="absolute top-0"
          v-model:current-page="pager.pageNumber"
          v-model:page-size="pager.pageSize"
          :page-sizes="[5, 10, 15, 50]"
          :small="false"
          :disabled="false"
          :background="true"
          layout=",prev, pager, next"
          :total="pagerValues?.totalCount ?? 0"
          @current-change="getEntities()"
          @size-change="getEntities()"
        />
      </div>
    </div>
  </div>

  <el-dialog
    v-model="confirmDeleteDialog"
    title="Delete language"
    width="30%"
    center
  >
    Do you really want to delete course
    <span class="font-bold"> {{ focusedItem?.name }}</span> with all its data?
    <template #footer>
      <span class="dialog-footer">
        <el-button plain @click="confirmDeleteDialog = false">Cancel</el-button>
        <el-button type="danger" plain @click="handleDelete">
          Delete
        </el-button>
      </span>
    </template>
  </el-dialog>

  <el-dialog
    v-model="editDescriptionDialog"
    title="Edit course description"
    width="70%"
    center
  >
    <el-form
      class="my-2"
      label-position="left"
      label-width="auto"
      ref="formEditCourseDescriptionRef"
      :model="courseDescriptionModel"
    >
      <span class="font-bold text-lg"
        >Send application issue to system administrators</span
      >
      <el-form-item
        class="text-lg mt-2"
        prop="description"
        label="Course description"
      >
        <el-input
          v-model="courseDescriptionModel.description"
          type="textarea"
          placeholder="Describe an issue/issues with application"
          autosize
        />
      </el-form-item>
    </el-form>
    <template #footer>
      <span class="dialog-footer">
        <el-button plain @click="editDescriptionDialog = false"
          >Cancel</el-button
        >
        <el-button
          type="success"
          plain
          @click="submitCourseDescriptionEdit(formEditCourseDescriptionRef)"
          >Edit</el-button
        >
      </span>
    </template>
  </el-dialog>
</template>

<style scoped></style>
