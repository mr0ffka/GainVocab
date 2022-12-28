import { ICourseDataFilterModel } from "@/services/admin/types";
import { IPagerParams } from "@/services/common/types";
import { ref } from "vue";
import { defineStore } from "pinia";

export const useAdminCourseDataStore = defineStore(
  "adminCourseDataStore",
  () => {
    const isSearching = ref(false);
    const selectedCourseId = ref<string>("");
    const pager = ref<IPagerParams>({
      pageSize: 10,
      pageNumber: 1,
      sortBy: "",
      sortDirection: "ASC",
    });
    const filter = ref<ICourseDataFilterModel>({
      publicId: "",
      source: "",
      translation: "",
    });

    function resetFilters() {
      filter.value.publicId = "";
      filter.value.source = "";
      filter.value.translation = "";
    }

    return {
      pager,
      filter,
      isSearching,
      selectedCourseId,
      resetFilters,
    };
  }
);
