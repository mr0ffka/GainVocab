<script setup lang="ts">
import AdminMenu from "@/components/admin/AdminMenu.vue";
import {
  getSupportIssueList,
  removeSupportIssue,
  resolveSupportIssue,
  getUserOptionsList,
  getSupportIssuesTypesOptionsList,
} from "@/services/admin/adminApi";
import { storeToRefs } from "pinia";
import { onMounted, ref } from "vue";
import {
  ISupportIssueListItemModel,
  ISupportIssueTypeOptionModel,
  IUserOptionModel,
} from "@/services/admin/types";
import { IPagedResult, IPager } from "@/services/common/types";
import { RefreshRight, Search } from "@element-plus/icons-vue";
import { ElMessage } from "element-plus";
import Header from "@/components/common/Header.vue";
import { useSupportIssueStore } from "@/store/adminSupportIssueStore";
import { DateTime } from "luxon";
import router from "@/router";
import { useRoute } from "vue-router";

const route = useRoute();
const store = useSupportIssueStore();
const { filter, pager, isSearching } = storeToRefs(store);
const confirmDeleteDialog = ref(false);
const confirmResolveDialog = ref(false);
const focusedItem = ref<ISupportIssueListItemModel | null>();
let entities = ref<ISupportIssueListItemModel[]>();
let pagerValues = ref<IPager>();
const usersOptions = ref<IUserOptionModel[] | null>();
const issueTypesOptions = ref<ISupportIssueTypeOptionModel[] | null>();

const getEntities = async () => {
  isSearching.value = true;
  await getSupportIssueList(filter.value, pager.value)
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

const resolveIssue = async (id: string) =>
  await resolveSupportIssue(id)
    .then((data: any) => {
      ElMessage({
        showClose: true,
        message: "Issue has been resolved",
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

const deleteEntity = async (id: string) =>
  await removeSupportIssue(id)
    .then((data: any) => {
      ElMessage({
        showClose: true,
        message: "Issue has been deleted",
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

const getUsersOptions = async () => {
  await getUserOptionsList()
    .then((data: IUserOptionModel[]) => {
      let options: IUserOptionModel[] = [];
      data.forEach((el) => {
        options.push(el);
      });
      usersOptions.value = options;
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

const getIssueTypesOptions = async () => {
  await getSupportIssuesTypesOptionsList()
    .then((data: ISupportIssueTypeOptionModel[]) => {
      let options: ISupportIssueTypeOptionModel[] = [];
      data.forEach((el) => {
        options.push(el);
      });
      issueTypesOptions.value = options;
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

onMounted(() => {
  if (route.query.issueId !== undefined) {
    filter.value.publicId = route.query.issueId!.toString();
  }
  getEntities();
  getUsersOptions();
  getIssueTypesOptions();
});

const formatDate = (date: string) => {
  const dateObject = DateTime.fromISO(date);
  return dateObject.toFormat("MMMM dd, yyyy");
};

const getTableDate = (date: any) => {
  if (date !== undefined) {
    return formatDate(date);
  }
  return "";
};

const handleResolveDialog = (row: ISupportIssueListItemModel) => {
  focusedItem.value = row;
  confirmResolveDialog.value = true;
};

const handleResolve = () => {
  resolveIssue(focusedItem.value?.id ?? "");
  confirmResolveDialog.value = false;
  focusedItem.value = null;
};

const handleDeleteDialog = (row: ISupportIssueListItemModel) => {
  focusedItem.value = row;
  confirmDeleteDialog.value = true;
};
const handleDelete = () => {
  deleteEntity(focusedItem.value?.id ?? "");
  confirmDeleteDialog.value = false;
  focusedItem.value = null;
};
const handleEdit = (row: ISupportIssueListItemModel) => {
  router.push({
    name: "data-edit",
    params: { publicId: row.issueEntity.entityId },
  });
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
        <div
          class="grid md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 2xl:grid-cols-5 gap-2"
        >
          <el-input v-model="filter.publicId" placeholder="Issue Id" />
          <el-select
            v-model="filter.isResolved"
            multiple
            collapse-tags
            collapse-tags-tooltip
            placeholder="Is resolved?"
            clearable
            class="min-w-fit"
            @clear="getEntities"
          >
            <el-option
              v-for="item in ['true', 'false']"
              :key="item"
              :label="store.boolToStringHandler(item.toLowerCase() === 'true')"
              :value="item"
            />
          </el-select>
          <el-select
            v-model="filter.typeId"
            multiple
            collapse-tags
            collapse-tags-tooltip
            placeholder="Issue type"
            clearable
            class="min-w-fit"
            @clear="getEntities"
          >
            <el-option
              v-for="item in issueTypesOptions"
              :key="item.publicId"
              :label="item.name"
              :value="item.publicId"
            />
          </el-select>
          <el-select
            v-model="filter.reporterId"
            multiple
            collapse-tags
            collapse-tags-tooltip
            placeholder="Reporter"
            clearable
            class="min-w-fit"
            @clear="getEntities"
          >
            <el-option
              v-for="item in usersOptions"
              :key="item.id"
              :label="item.email"
              :value="item.id"
            />
          </el-select>
          <el-date-picker
            v-model="filter.created"
            type="daterange"
            start-placeholder="Created date from"
            end-placeholder="Created date to"
            class="col-span-2 min-w-fit"
            value-format="DD/MM/YYYY"
            @clear="getEntities"
          />
          <el-date-picker
            v-model="filter.updated"
            type="daterange"
            start-placeholder="Updated date from"
            end-placeholder="Updated date to"
            class="col-span-2 min-w-fit"
            value-format="DD/MM/YYYY"
            @clear="getEntities"
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
            class="mb-2 mr-2 p-3 font-bold right !ml-auto"
            plain
            type="info"
            @click="resetFilters"
            ><el-icon><RefreshRight /></el-icon>&nbsp;Reset filters</el-button
          >
        </div>
      </div>
      <el-table
        :data="entities ?? []"
        :default-sort="{ prop: 'typeName', order: 'descending' }"
        :flexible="true"
        :border="true"
        :stripe="true"
        class="mt-2"
      >
        <el-table-column type="expand">
          <template #default="scope">
            <div class="mx-2">
              <el-descriptions :column="1" :border="true" class="mt-2">
                <el-descriptions-item label="Issue id">{{
                  scope.row.id
                }}</el-descriptions-item>
                <el-descriptions-item label="Issue type">
                  <el-tag disable-transitions>
                    {{ scope.row.typeName }}
                  </el-tag>
                </el-descriptions-item>
                <el-descriptions-item label="Is resolved?">
                  {{ store.boolToStringHandler(scope.row.isResolved) }}
                </el-descriptions-item>
                <el-descriptions-item label="Created at">{{
                  getTableDate(scope.row.createdAt)
                }}</el-descriptions-item>
                <el-descriptions-item label="Updated at">{{
                  getTableDate(scope.row.updatedAt)
                }}</el-descriptions-item>
                <el-descriptions-item label="Reporter">
                  <el-table
                    :data="[scope.row.reporter]"
                    :border="true"
                    :stripe="true"
                  >
                    <el-table-column
                      label-class-name="font-black"
                      prop="firstName"
                      label="First Name"
                      width=""
                    />
                    <el-table-column
                      label-class-name="font-black"
                      prop="lastName"
                      label="Last Name"
                      width=""
                    />
                    <el-table-column
                      label-class-name="font-black"
                      prop="email"
                      label="Email"
                      width=""
                    />
                    <el-table-column
                      label-class-name="font-black"
                      prop="roles"
                      label="Roles"
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
                      prop="courses"
                      label="Courses"
                      width=""
                    >
                      <template #default="scope">
                        <el-tag
                          v-for="course in scope.row.courses"
                          class="mr-2"
                          disable-transitions
                          >{{ course }}
                        </el-tag>
                      </template>
                    </el-table-column>
                  </el-table>
                </el-descriptions-item>
                <el-descriptions-item
                  v-if="scope.row.issueEntity.courseName !== null"
                  label="Issue"
                >
                  <el-table
                    :data="[scope.row.issueEntity]"
                    :border="true"
                    :stripe="true"
                  >
                    <el-table-column
                      label-class-name="font-black"
                      prop="courseName"
                      label="Course name"
                      width=""
                    />
                    <el-table-column
                      label-class-name="font-black"
                      prop="languageFrom"
                      label="Language From"
                      width=""
                    />
                    <el-table-column
                      label-class-name="font-black"
                      prop="languageTo"
                      label="Language To"
                      width=""
                    />
                    <el-table-column
                      label-class-name="font-black"
                      prop="source"
                      label="Source text"
                      width=""
                    />
                    <el-table-column
                      label-class-name="font-black"
                      prop="translation"
                      label="Translation text"
                      width=""
                    />
                  </el-table>
                </el-descriptions-item>
                <el-descriptions-item label="Message">{{
                  scope.row.message
                }}</el-descriptions-item>
              </el-descriptions>
              <div class="mt-2 flex flex-row">
                <el-button
                  type="success"
                  plain
                  @click="handleResolveDialog(scope.row)"
                  >Resolve</el-button
                >
                <el-button
                  v-if="scope.row.issueEntity.courseName !== null"
                  type="info"
                  plain
                  @click="handleEdit(scope.row)"
                  >Edit entity</el-button
                >
                <el-button
                  type="danger"
                  plain
                  @click="handleDeleteDialog(scope.row)"
                  >Delete</el-button
                >
              </div>
            </div>
          </template>
        </el-table-column>
        <el-table-column
          label-class-name="font-black"
          prop="typeName"
          label="Issue type"
          sortable
          width=""
        >
          <template #default="scope">
            <el-tag class="mr-2" disable-transitions
              >{{ scope.row.typeName }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column
          label-class-name="font-black"
          prop="reporter.email"
          label="Reporter"
          sortable
          width=""
        />
        <el-table-column
          label-class-name="font-black"
          prop="isResolved"
          label="Is resolved?"
          sortable
          width=""
        >
          <template #default="scope">
            {{ store.boolToStringHandler(scope.row.isResolved) }}
          </template>
        </el-table-column>
        <el-table-column
          label-class-name="font-black"
          label="Issue created at"
          sortable
          width=""
        >
          <template #default="scope">
            {{ getTableDate(scope.row.createdAt) }}
          </template>
        </el-table-column>
        <el-table-column
          label-class-name="font-black"
          label="Issue updated at"
          sortable
          width=""
        >
          <template #default="scope">
            {{ getTableDate(scope.row.updatedAt) }}
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
    title="Delete issue"
    width="30%"
    center
  >
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
    v-model="confirmResolveDialog"
    title="Resolve issue"
    width="30%"
    center
  >
    <template #footer>
      <span class="dialog-footer">
        <el-button plain @click="confirmDeleteDialog = false">Cancel</el-button>
        <el-button type="success" plain @click="handleResolve">
          Resolve
        </el-button>
      </span>
    </template>
  </el-dialog>
</template>

<style scoped></style>
