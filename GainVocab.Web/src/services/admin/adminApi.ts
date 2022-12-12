import { toURLSearchParams } from "../../helpers/axios";
import { GenericResponse } from "../common/types";
import {
  ICourseFilterModel,
  ILanguageAddModel,
  ILanguageFilterModel,
  ILanguageModel,
  IUserAddModel,
  IUserDetailsModel,
  ICourseItemModel,
  ICourseAddModel,
} from "./types";

import type { IPagerParams, IPagedResult } from "../common/types";
import type { IUserFilterModel } from "../admin/types";
import { api } from "@/helpers/axios";

export const getListUser = async (
  filter: IUserFilterModel,
  pager: IPagerParams
) => {
  return (
    await api.get<IPagedResult>("users", {
      params: toURLSearchParams({ ...filter, ...pager }),
    })
  ).data;
};

export const addUser = async (user: IUserAddModel) => {
  return (await api.post<GenericResponse>("users/add", user)).data;
};

export const getUser = async (id: string) => {
  return (await api.get<IUserAddModel>(`users/${id}`)).data;
};

export const getUserDetails = async (id: string) => {
  return (await api.get<IUserDetailsModel>(`users/details/${id}`)).data;
};

export const removeUser = async (id: string) => {
  return (await api.delete(`users/${id}`)).data;
};

export const updateUser = async (id: string, model: IUserAddModel) => {
  return (await api.patch<IUserAddModel>(`users/${id}`, model)).data;
};

export const addLanguage = async (entity: ILanguageAddModel) => {
  return (await api.post<GenericResponse>("course/language/add", entity)).data;
};

export const getListLanguage = async (
  filter: ILanguageFilterModel,
  pager: IPagerParams
) => {
  return (
    await api.get<IPagedResult>("course/language", {
      params: toURLSearchParams({ ...filter, ...pager }),
    })
  ).data;
};

export const removeLanguage = async (publicId: string) => {
  return (await api.delete(`course/language/${publicId}`)).data;
};

export const getLanguageOptionsList = async () => {
  return (await api.get<ICourseItemModel[]>("course/language/options")).data;
};

export const getListCourse = async (
  filter: ICourseFilterModel,
  pager: IPagerParams
) => {
  return (
    await api.get<IPagedResult>("course", {
      params: toURLSearchParams({ ...filter, ...pager }),
    })
  ).data;
};

export const getCoursesOptionsList = async () => {
  return (await api.get<ICourseItemModel[]>("course/options")).data;
};

export const addCourse = async (entity: ICourseAddModel) => {
  return (await api.post<GenericResponse>("course/add", entity)).data;
};

export const removeCourse = async (publicId: string) => {
  return (await api.delete(`course/${publicId}`)).data;
};