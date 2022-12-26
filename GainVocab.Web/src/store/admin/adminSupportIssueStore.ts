import { IPagerParams } from "@/services/common/types";
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

  const boolToStringHandler = (value: boolean) => {
    if (value) return "Yes";
    return "No";
  };

  const filter = ref<ISupportIssueFilterModel>({
    publicId: "",
    isResolved: [],
    reporterId: [],
    typeId: [],
    created: "",
    updated: "",
  });

  function resetFilters() {
    filter.value.publicId = "";
    filter.value.isResolved = [];
    filter.value.reporterId = [];
    filter.value.typeId = [];
    filter.value.created = "";
    filter.value.updated = "";
  }

  return { pager, filter, isSearching, resetFilters, boolToStringHandler };
});
