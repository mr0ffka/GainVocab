import { toURLSearchParams } from "../../helpers/axios";
import { GenericResponse } from "../common/types";
import {
  IAddUserToCourseModel,
  ISupportApplicationIssueModel,
  IUserProfileEditModel,
} from "./types";

import type { IPagerParams, IPagedResult } from "../common/types";
import type {
  ICourseFilterModel,
  IUserDetailsModel,
  IUserFilterModel,
} from "../admin/types";
import { api } from "@/helpers/axios";

export const sendSupportApplicationIssue = async (
  model: ISupportApplicationIssueModel
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
