import merge from "lodash/merge";
import axios from "axios";

import type { IPagerParams, IPagedResult } from "../common/types";
import type { IFilterModel } from "../user/types";

const userApi = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
  withCredentials: true,
});

userApi.defaults.headers.common["Content-Type"] = "application/json";

export const getListUser = async (
  filter: IFilterModel,
  pager: IPagerParams
) => {
  return (
    await userApi.get<IPagedResult>("users", {
      params: merge({}, filter, pager),
    })
  ).data;
};
