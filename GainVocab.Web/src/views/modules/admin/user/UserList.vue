<script setup lang="ts">
import AdminMenu from "@/components/admin/AdminMenu.vue";
import { getListUser, removeUser } from "@/services/user/userApi";
import { useUserStore } from "@/store/userStore";
import { storeToRefs } from "pinia";
import { onMounted, ref } from "vue";
import { IUserModel } from "@/services/user/types";
import { IPagedResult, IPager } from "@/services/common/types";
import router from "@/router";
import { Plus, RefreshRight, Search } from "@element-plus/icons-vue";
import { ElMessage } from "element-plus";
import Header from "@/components/common/Header.vue";

const userStore = useUserStore();
const { filter, pager, isSearching } = storeToRefs(userStore);
const confirmDeleteDialog = ref(false);
const focusedItem = ref<IUserModel | null>();
let users = ref<IUserModel[]>();
let pagerValues = ref<IPager>();
const userRoleOptions = userStore.getUserRoleOptions();

const getUsers = () => {
  isSearching.value = true;
  getListUser(filter.value, pager.value)
    .then((data: IPagedResult) => {
      isSearching.value = false;
      users.value = data.items;
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

const deleteUser = async (id: string) =>
  await removeUser(id)
    .then((data: any) => {
      ElMessage({
        showClose: true,
        message: "User has been deleted",
        type: "success",
      });
      getUsers();
    })
    .catch((error: any) => {
      ElMessage({
        showClose: true,
        message: error.response.data.Title,
        type: "error",
      });
    });

onMounted(() => {
  getUsers();
});

const rowClickDetails = (row: IUserModel, column: any) => {
  if (column.type != "not-clickable") {
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
  deleteUser(focusedItem.value?.id ?? "");
  confirmDeleteDialog.value = false;
  focusedItem.value = null;
};
const resetFilters = () => {
  userStore.resetFilters();
  getUsers();
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
            v-model="filter.firstName"
            class="mb-2"
            placeholder="First Name"
          />
          <el-input
            v-model="filter.lastName"
            class="mb-2 ml-2"
            placeholder="Last Name"
          />
          <el-select
            v-model="filter.roles"
            multiple
            collapse-tags
            collapse-tags-tooltip
            placeholder="Roles"
            clearable
            @clear="getUsers"
            class="ml-2 min-w-fit"
          >
            <el-option
              v-for="item in userRoleOptions"
              :key="item"
              :label="item"
              :value="item"
            />
          </el-select>
        </div>
        <div class="flex flex-row !ml-auto">
          <el-button
            class="mb-2 mr-2 p-3 font-bold right !ml-auto"
            plain
            :loading="isSearching"
            @click="getUsers"
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
            @click="router.push({ name: 'user-add' })"
            ><el-icon><Plus /></el-icon>&nbsp;Add user</el-button
          >
        </div>
      </div>
      <el-table
        :data="users ?? []"
        :default-sort="{ prop: 'firstName', order: 'descending' }"
        :flexible="true"
        :border="true"
        :stripe="true"
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
          prop="roles"
          label="Roles"
          sortable
          width=""
        >
          <template #default="scope">
            <el-tag
              v-for="role in scope.row.roles"
              class="mr-2"
              disable-transitions
              >{{ role }}
            </el-tag>
          </template>
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
          @current-change="getUsers()"
          @size-change="getUsers()"
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
          @current-change="getUsers()"
          @size-change="getUsers()"
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
