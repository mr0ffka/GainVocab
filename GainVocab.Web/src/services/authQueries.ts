import { useQuery } from "@tanstack/vue-query";
import { getCurrUser, refreshAccessTokenFn } from "./authApi";

export const useAuthUser = () => useQuery(["authUser"], getCurrUser);
export const useAuthRefreshToken = () =>
  useQuery(["authRefreshToken"], refreshAccessTokenFn);
