import { ref } from "vue";
import { defineStore, mapActions, mapState, mapStores } from "pinia";

export const useAuthMenuStore = defineStore("authMenuStore", () => {
  const isMenuCollapsed = ref(false);
  function changeMenuCollapsed() {
    isMenuCollapsed.value = !isMenuCollapsed.value;
  }

  return { isMenuCollapsed, changeMenuCollapsed };
});
