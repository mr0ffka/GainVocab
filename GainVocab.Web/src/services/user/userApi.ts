import { toURLSearchParams } from "../../helpers/axios";
import { GenericResponse } from "../common/types";
import {
  IAddUserToCourseModel,
  ILearnCourseCheckModel,
  ILearnCourseCheckResponseModel,
  ILearnCourseGetModel,
  ILearnCourseModel,
  ILearnCourseNextModel,
  ILearnCourseNextResponseModel,
  ILearnCourseSendModel,
  ISupportApplicationIssueModel,
  ISupportCourseDataIssueModel,
  IUserProfileEditModel,
} from "./types";

import type { IPagerParams, IPagedResult } from "../common/types";
import type {
  ICourseFilterModel,
  IUserDetailsModel,
  IUserFilterModel,
} from "../admin/types";
import { api } from "@/helpers/axios";

export const sendSupportIssue = async (
  model: ISupportApplicationIssueModel | ISupportCourseDataIssueModel
) => {
  return (await api.post<GenericResponse>("support/add", model)).data;
};

export const removeMe = async (id: string) => {
  return (await api.delete(`users/me/${id}`)).data;
};

export const getMeDetails = async (id: string) => {
  return (await api.get<IUserDetailsModel>(`users/me/details/${id}`)).data;
};

export const updateMe = async (id: string, model: IUserProfileEditModel) => {
  return (await api.patch(`users/me/${id}`, model)).data;
};

export const getAvailableCourseList = async (
  userId: string,
  filter: ICourseFilterModel
) => {
  return (
    await api.get<IPagedResult>("course/me/available", {
      params: toURLSearchParams({ userId, ...filter }),
    })
  ).data;
};

export const addUserToCourse = async (model: IAddUserToCourseModel) => {
  return (await api.post<GenericResponse>("course/me/add", model)).data;
};

export const getActiveCourseList = async (
  userId: string,
  filter: ICourseFilterModel
) => {
  return (
    await api.get<IPagedResult>("course/me/active", {
      params: toURLSearchParams({ userId, ...filter }),
    })
  ).data;
};

export const getLearnData = async (model: ILearnCourseGetModel) => {
  return (
    await api.get<ILearnCourseModel>("course/get-learn", {
      params: toURLSearchParams({ ...model }),
    })
  ).data;
};

export const learnCourseCheck = async (model: ILearnCourseCheckModel) => {
  return (await api.post("course/learn/check", model)).data;
};

export const getNextData = async (model: ILearnCourseNextModel) => {
  return (
    await api.get<ILearnCourseNextResponseModel>("course/learn/next", {
      params: toURLSearchParams({ ...model }),
    })
  ).data;
};
