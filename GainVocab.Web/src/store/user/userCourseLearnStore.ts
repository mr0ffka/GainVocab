import { ref } from "vue";
import { defineStore } from "pinia";
import { ICourseFilterModel } from "@/services/admin/types";
import { IPagerParams } from "@/services/common/types";
import {
  ILearnCourseModel,
  ILearnCourseNextResponseModel,
} from "@/services/user/types";

export const useUserCourseLearnStore = defineStore(
  "userCourseLearnStore",
  () => {
    const currentCourse = ref<ILearnCourseModel>();

    return { currentCourse };
  }
);
