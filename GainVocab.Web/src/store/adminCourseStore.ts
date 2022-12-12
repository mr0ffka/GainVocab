import { useAdminLanguageStore } from "./adminLanguageStore";
import { ICourseModel, ILanguageModel } from "./../services/admin/types";
import { IPagerParams } from "../services/common/types";
import { ref } from "vue";
import { defineStore } from "pinia";
import { ICourseFilterModel } from "@/services/admin/types";

export const useAdminCourseStore = defineStore("adminCourseStore", () => {
  const languageStore = useAdminLanguageStore();
  const isSearching = ref(false);
  const empty = ref<ICourseModel>({
    id: "",
    name: "",
    languageFrom: languageStore.empty,
    languageTo: languageStore.empty,
  });
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

  //   function getUserRoleOptions() {
  //     const options = ["Administrator", "User"];
  //     return options;
  //   }

  return { pager, empty, filter, isSearching, resetFilters };
});
