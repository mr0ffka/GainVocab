import { ref } from "vue";
import { defineStore } from "pinia";
import { ICourseFilterModel } from "@/services/admin/types";
import { IPagerParams } from "@/services/common/types";

export const useAdminCourseStore = defineStore("adminCourseStore", () => {
  const isSearching = ref(false);
  const pager = ref<IPagerParams>({
    pageSize: 10,
    pageNumber: 1,
    sortBy: "",
    sortDirection: "ASC",
  });
  const filter = ref<ICourseFilterModel>({
    name: "",
    languageFrom: "",
    languageTo: "",
  });

  function resetFilters() {
    filter.value.name = "";
    filter.value.languageFrom = "";
    filter.value.languageTo = "";
  }

  return { pager, filter, isSearching, resetFilters };
});
