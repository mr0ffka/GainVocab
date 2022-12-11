import { toURLSearchParams } from "./../../helpers/axios";
import { GenericResponse } from "./../common/types";
import { IUserAddModel, IUserDetailsModel } from "./types";

import type { IPagerParams, IPagedResult } from "../common/types";
import type { IFilterModel } from "../user/types";
import { api as userApi } from "@/helpers/axios";

export const getListUser = async (
  filter: IFilterModel,
  pager: IPagerParams
) => {
  return (
    await userApi.get<IPagedResult>("users", {
      params: toURLSearchParams({ ...filter, ...pager }),
    })
  ).data;
};

export const addUser = async (user: IUserAddModel) => {
  return (await userApi.post<GenericResponse>("users/add", user)).data;
};

export const getUser = async (id: string) => {
  return (await userApi.get<IUserAddModel>(`users/${id}`)).data;
};

export const getUserDetails = async (id: string) => {
  return (await userApi.get<IUserDetailsModel>(`users/details/${id}`)).data;
};

export const removeUser = async (id: string) => {
  return (await userApi.delete(`users/${id}`)).data;
};

export const updateUser = async (id: string, model: IUserAddModel) => {
  return (await userApi.patch<IUserAddModel>(`users/${id}`, model)).data;
};
