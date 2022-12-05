<script setup lang="ts">
import AdminMenu from "@/components/admin/AdminMenu.vue";
import { getListUser } from "@/services/user/userApi";
import { useUserStore } from "@/store/userStore";
import { useMutation, useQuery } from "@tanstack/vue-query";
import { storeToRefs } from "pinia";
import { onMounted, ref } from "vue";
import { IUserModel } from "@/services/user/types";
import { IPagedResult, IPager } from "@/services/common/types";
import router from "@/router";
import { Plus, RefreshRight, Search } from "@element-plus/icons-vue";
import { ElMessage } from "element-plus";

const userStore = useUserStore();
const { filter, pager } = storeToRefs(userStore);
const confirmDeleteDialog = ref(false);
const focusedItem = ref<IUserModel | null>();
let users = ref<IUserModel[]>();
let pagerValues = ref<IPager>();

const getUsers = useMutation(
  async () => await getListUser(filter.value, pager.value),
  {
    onError: (error: any) => {
      ElMessage({
        showClose: true,
        message: error.response.data.Title,
        type: "error",
      });
    },
    onSuccess: (data: IPagedResult) => {
      users.value = data.items;
      pagerValues.value = {
        pageNumber: data.pageNumber,
        totalCount: data.totalCount,
        recordNumber: data.recordNumber,
      };
    },
  }
);

onMounted(() => {
  getUsers.mutate();
});

const rowClickDetails = (row: IUserModel, column: any) => {
  if (column.index != "col-operations") {
    router.push({ name: "user-details", params: { id: row.id } });
  }
};
const handleDetails = (row: IUserModel) => {
  router.push({ name: "user-details", params: { id: row.id } });
};
const handleEdit = (row: IUserModel) => {
  router.push({ name: "user-edit", params: { id: row.id } });
};
const handleDeleteDialog = (row: IUserModel) => {
  focusedItem.value = row;
  confirmDeleteDialog.value = true;
};
const handleDelete = () => {
  // delete logic
  confirmDeleteDialog.value = false;
  focusedItem.value = null;
};
const resetFilters = () => {
  userStore.resetFilters();
  getUsers.mutate();
};
</script>

<template>
  <div class="flex grow">
    <AdminMenu />
    <div class="grow flex flex-col">
      <div class="flex w-100">
        <div class="flex flex-row">
          <el-input
            v-model="filter.firstName"
            class="w-50 m-2"
            placeholder="First Name"
          />
          <el-input
            v-model="filter.lastName"
            class="w-50 m-2"
            placeholder="Last Name"
          />
        </div>
        <div class="!ml-auto">
          <el-button
            class="my-2 mr-2 p-3 font-bold right !ml-auto"
            plain
            @click="getUsers.mutate()"
            ><el-icon><Search /></el-icon>&nbsp;Search</el-button
          >
          <el-button
            class="my-2 mr-2 p-3 font-bold right !ml-auto"
            plain
            type="info"
            @click="resetFilters"
            ><el-icon><RefreshRight /></el-icon>&nbsp;Reset filters</el-button
          >
          <el-button
            class="my-2 mr-2 p-3 font-bold right !ml-auto"
            type="success"
            plain
            @click="router.push({ name: 'user-add' })"
            ><el-icon><Plus /></el-icon>&nbsp;Add user</el-button
          >
        </div>
      </div>
      <el-table
        class="px-2 overflow-x-hidden"
        :stripe="true"
        :data="users ?? []"
        :default-sort="{ prop: 'firstName', order: 'descending' }"
        :flexible="true"
        :border="true"
        @row-click="rowClickDetails"
      >
        <el-table-column
          label-class-name="font-black"
          prop="firstName"
          label="First Name"
          sortable
          width=""
        />
        <el-table-column
          label-class-name="font-black"
          prop="lastName"
          label="Last Name"
          sortable
          width=""
        />
        <el-table-column
          label-class-name="font-black"
          prop="email"
          label="Email"
          sortable
          width=""
        />
        <el-table-column
          label-class-name="font-black"
          fixed="right"
          label="Operations"
          index="col-operations"
        >
          <template #default="scope">
            <el-button
              size="small"
              plain
              type="info"
              @click="handleDetails(scope.row)"
              >Details</el-button
            >
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
          @current-change="getUsers.mutate()"
          @size-change="getUsers.mutate()"
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
          @current-change="getUsers.mutate()"
          @size-change="getUsers.mutate()"
        />
      </div>
    </div>
  </div>

  <el-dialog
    v-model="confirmDeleteDialog"
    title="Delete user"
    width="30%"
    center
  >
    Do you really want to delete user:
    <span class="font-bold">
      {{ focusedItem?.firstName }} {{ focusedItem?.lastName }}</span
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
