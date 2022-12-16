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
  ICourseDataAddModel,
  ICourseDataUpdateModel,
  ICourseDataFilterModel,
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

export const getRoleOptionsList = async () => {
  return (await api.get<string[]>("users/roles/options")).data;
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

export const addCourseData = async (entity: ICourseDataAddModel) => {
  return (await api.post<GenericResponse>("course-data/add", entity)).data;
};

export const updateCourseData = async (
  id: string,
  model: ICourseDataUpdateModel
) => {
  return (await api.patch<GenericResponse>(`course-data/${id}`, model)).data;
};

export const getCourseData = async (publicId: string) => {
  return (await api.get<ICourseDataAddModel>(`course-data/${publicId}`)).data;
};

export const getListCourseData = async (
  coursePublicId: string,
  filter: ICourseDataFilterModel,
  pager: IPagerParams
) => {
  let request = await api.get<IPagedResult>("course-data", {
    params: toURLSearchParams({ coursePublicId, ...filter, ...pager }),
  });
  return request.data;
};

export const removeCourseData = async (publicId: string) => {
  return (await api.delete(`course-data/${publicId}`)).data;
};

export const getUserCount = async () => {
  return (await api.get<number>("users/count")).data;
};

export const getLanguageCount = async () => {
  return (await api.get<number>("course/language/count")).data;
};

export const getCourseCount = async () => {
  return (await api.get<number>("course/count")).data;
};

export const getCourseDataCount = async () => {
  return (await api.get<number>("course-data/count")).data;
};

export const getSupportTicketsCount = async () => {
  return (await api.get<number>("users/count")).data;
};
