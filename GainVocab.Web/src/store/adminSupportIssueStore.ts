import { IPagerParams } from "../services/common/types";
import { ref } from "vue";
import { defineStore } from "pinia";
import { ISupportIssueFilterModel } from "@/services/admin/types";

export const useSupportIssueStore = defineStore("supportIssueStore", () => {
  const isSearching = ref(false);
  const pager = ref<IPagerParams>({
    pageSize: 10,
    pageNumber: 1,
    sortBy: "",
    sortDirection: "ASC",
  });
  const filter = ref<ISupportIssueFilterModel>({
    isResolved: [],
    reporterId: [],
    typeId: [],
    createdFrom: "",
    createdTo: "",
    updatedFrom: "",
    updatedTo: "",
  });

  function resetFilters() {
    filter.value.isResolved = [];
    filter.value.reporterId = [];
    filter.value.typeId = [];
    filter.value.createdFrom = "";
    filter.value.createdTo = "";
    filter.value.updatedFrom = "";
    filter.value.updatedTo = "";
  }

  return { pager, filter, isSearching, resetFilters };
});
