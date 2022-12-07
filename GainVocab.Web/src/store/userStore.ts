import { IPagerParams } from "./../services/common/types";
import { ref } from "vue";
import { defineStore, mapActions, mapState, mapStores } from "pinia";
import { IFilterModel, USER_ROLES } from "@/services/user/types";

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
    roles: [],
  });

  function resetFilters() {
    filter.value.firstName = "";
    filter.value.lastName = "";
    filter.value.roles = [];
  }

  function getUserRoleKeyValuePair() {
    const options = USER_ROLES.map((userRole, index) => ({
      key: index,
      value: userRole,
    }));

    return options;
  }

  return { pager, filter, resetFilters, getUserRoleKeyValuePair };
});
