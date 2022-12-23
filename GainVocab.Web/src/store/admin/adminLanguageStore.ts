import { IPagerParams } from "../services/common/types";
import { ref } from "vue";
import { defineStore } from "pinia";
import { ILanguageFilterModel, ILanguageModel } from "@/services/admin/types";

export const useAdminLanguageStore = defineStore("adminLanguageStore", () => {
  const isSearching = ref(false);
  const empty = ref<ILanguageModel>({
    id: "",
    name: "",
  });
  const pager = ref<IPagerParams>({
    pageSize: 10,
    pageNumber: 1,
    sortBy: "",
    sortDirection: "ASC",
  });
  const filter = ref<ILanguageFilterModel>({
    name: "",
    courses: [],
  });

  function resetFilters() {
    filter.value.name = "";
    filter.value.courses = [];
  }
  //   function getUserRoleOptions() {
  //     const options = ["Administrator", "User"];
  //     return options;
  //   }

  return { pager, filter, isSearching, empty, resetFilters };
});
