<script setup lang="ts">
import AdminMenu from "@/components/admin/AdminMenu.vue";
import {
  getCoursesOptionsList,
  getListCourseData,
  removeCourseData,
} from "@/services/admin/adminApi";
import { storeToRefs } from "pinia";
import { onMounted, ref } from "vue";
import { ICourseDataListModel, ICourseItemModel } from "@/services/admin/types";
import { IPagedResult, IPager } from "@/services/common/types";
import router from "@/router";
import { Plus, RefreshRight, Search } from "@element-plus/icons-vue";
import { ElMessage } from "element-plus";
import Header from "@/components/common/Header.vue";
import { useAdminCourseDataStore } from "@/store/admin/adminCourseDataStore";
import { useRoute } from "vue-router";

const store = useAdminCourseDataStore();
const route = useRoute();
const { filter, pager, isSearching } = storeToRefs(store);
const confirmDeleteDialog = ref(false);
const selectedCourseId = ref<string>("");
const focusedItem = ref<ICourseDataListModel | null>();
let entities = ref<ICourseDataListModel[]>();
let pagerValues = ref<IPager>();
const coursesOptions = ref<ICourseItemModel[] | null>();

const getEntities = () => {
  isSearching.value = true;
  getListCourseData(selectedCourseId.value, filter.value, pager.value)
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
  await removeCourseData(id)
    .then((data: any) => {
      ElMessage({
        showClose: true,
        message: "Data entry has been deleted",
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

const loadData = () => {
  getEntities();
};

onMounted(() => {
  getCoursesOptions();
  if (route.query.coursePublicId !== undefined) {
    selectedCourseId.value = route.query.coursePublicId!.toString();
  }
  if (selectedCourseId.value != "") {
    loadData();
  }
});
const handleEdit = (row: ICourseDataListModel) => {
  router.push({ name: "data-edit", params: { publicId: row.publicId } });
};
const handleDeleteDialog = (row: ICourseDataListModel) => {
  focusedItem.value = row;
  confirmDeleteDialog.value = true;
};
const handleDelete = () => {
  deleteEntity(focusedItem.value?.publicId ?? "");
  confirmDeleteDialog.value = false;
  focusedItem.value = null;
};
const resetFilters = () => {
  store.resetFilters();
  getEntities();
};

const rowClick = (row: ICourseDataListModel, column: any) => {
  if (column.type != "not-clickable") {
    router.push({ name: "data-edit", params: { publicId: row.publicId } });
  }
};
</script>

<template>
  <Header></Header>
  <div class="flex grow">
    <AdminMenu />
    <div class="grow flex flex-col p-2">
      <div class="flex">
        <div class="flex flex-row">
          <span class="inline-block align-middle font-bold"
            >Select course:</span
          >
          <el-select
            v-model="selectedCourseId"
            placeholder="Select"
            class="ml-2"
            @change="loadData"
          >
            <el-option
              v-for="item in coursesOptions"
              :key="item.id"
              :label="item.name"
              :value="item.id"
            />
          </el-select>
        </div>
        <div class="flex flex-row !ml-auto">
          <el-button
            class="mb-2 p-3 font-bold right !ml-auto"
            type="success"
            plain
            @click="router.push({ name: 'data-add' })"
            ><el-icon><Plus /></el-icon>&nbsp;Add data</el-button
          >
        </div>
      </div>
      <div v-if="selectedCourseId != ''">
        <div class="flex">
          <div class="flex flex-row">
            <el-input
              v-model="filter.publicId"
              class="mb-2"
              placeholder="Data id"
            />
          </div>
          <div class="flex flex-row">
            <el-input
              v-model="filter.source"
              class="mb-2 ml-2"
              placeholder="Source text"
            />
          </div>
          <div class="flex flex-row">
            <el-input
              v-model="filter.translation"
              class="mb-2 ml-2"
              placeholder="Translation text"
            />
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
              class="mb-2 p-3 font-bold right !ml-auto"
              plain
              type="info"
              @click="resetFilters"
              ><el-icon><RefreshRight /></el-icon>&nbsp;Reset filters</el-button
            >
          </div>
        </div>
        <el-table
          :data="entities ?? []"
          :default-sort="{ prop: 'name', order: 'descending' }"
          :flexible="true"
          :border="true"
          :stripe="true"
          @row-click="rowClick"
        >
          <el-table-column
            label-class-name="font-black"
            prop="publicId"
            label="Data Id"
            sortable
            width=""
          />
          <el-table-column
            label-class-name="font-black"
            prop="source"
            label="Source text"
            sortable
            width=""
          >
          </el-table-column>
          <el-table-column
            label-class-name="font-black"
            prop="translation"
            label="Translation text"
            sortable
            width=""
          >
          </el-table-column>
          <el-table-column
            label-class-name="font-black"
            prop="noExamples"
            label="No. examples"
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
                <el-button size="small" plain @click="handleEdit(scope.row)"
                  >Edit</el-button
                >
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
  </div>

  <el-dialog
    v-model="confirmDeleteDialog"
    title="Delete language"
    width="30%"
    center
  >
    Do you really want to delete data entry with all examples?
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
