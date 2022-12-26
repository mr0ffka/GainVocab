import { IPagerParams } from "@/services/common/types";
import { ref } from "vue";
import { defineStore } from "pinia";
import { IUserFilterModel } from "@/services/admin/types";

export const useAdminUserStore = defineStore("adminUserStore", () => {
  const isSearching = ref(false);
  const pager = ref<IPagerParams>({
    pageSize: 10,
    pageNumber: 1,
    sortBy: "",
    sortDirection: "ASC",
  });
  const filter = ref<IUserFilterModel>({
    firstName: "",
    lastName: "",
    roles: [],
    courses: [],
  });

  function resetFilters() {
    filter.value.firstName = "";
    filter.value.lastName = "";
    filter.value.roles = [];
    filter.value.courses = [];
  }

  return { pager, filter, isSearching, resetFilters };
});
