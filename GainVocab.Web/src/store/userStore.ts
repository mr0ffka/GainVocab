import { IPagerParams } from "./../services/common/types";
import { ref } from "vue";
import { defineStore, mapActions, mapState, mapStores } from "pinia";
import { IFilterModel } from "@/services/user/types";

export const useUserStore = defineStore("userStore", () => {
  const pager = ref<IPagerParams>({
    pageSize: 10,
    pageNumber: 1,
    sortBy: "",
    sortDirection: "ASC",
  });
  const filter = ref<IFilterModel>({
    firstName: "",
    lastName: "",
  });

  function resetFilters() {
    filter.value.firstName = "";
    filter.value.lastName = "";
  }

  return { pager, filter, resetFilters };
});
