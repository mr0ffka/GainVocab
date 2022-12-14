<script setup lang="ts">
import AdminMenu from "@/components/admin/AdminMenu.vue";
import {
  getLanguageOptionsList,
  getListCourse,
  removeCourse,
} from "@/services/admin/adminApi";
import { storeToRefs } from "pinia";
import { onMounted, ref } from "vue";
import { ICourseListModel, ILanguageModel } from "@/services/admin/types";
import { IPagedResult, IPager } from "@/services/common/types";
import router from "@/router";
import { Plus, RefreshRight, Search } from "@element-plus/icons-vue";
import { ElMessage } from "element-plus";
import Header from "@/components/common/Header.vue";
import { useAdminCourseStore } from "@/store/adminCourseStore";

const store = useAdminCourseStore();
const { filter, pager, isSearching } = storeToRefs(store);
const confirmDeleteDialog = ref(false);
const focusedItem = ref<ICourseListModel | null>();
let entities = ref<ICourseListModel[]>();
let pagerValues = ref<IPager>();
const languageOptions = ref<ILanguageModel[] | null>();

const getEntities = () => {
  isSearching.value = true;
  getListCourse(filter.value, pager.value)
    .then((data: IPagedResult) => {
      isSearching.value = false;
      entities.value = data.items;
      console.log(data);
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
        message: "Language has been deleted",
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
    Do you really want to delete course:
    <span class="font-bold"> {{ focusedItem?.name }}</span
    >?
    <template #footer>
      <span class="dialog-footer">
        <el-button plain @click="confirmDeleteDialog = false">Cancel</el-button>
        <el-button type="danger" plain @click="handleDelete">
          Delete
        </el-button>
      </span>
    </template>
  </el-dialog>
</template>

<style scoped></style>
