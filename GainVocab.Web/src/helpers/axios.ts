import { getCurrUser, refreshAccessTokenFn } from "@/services/auth/authApi";
import axios from "axios";
import { queryClient } from "./queryClient";

export const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
  withCredentials: true,
  headers: {
    "Content-Type": "application/json",
  },
});

api.interceptors.response.use(
  (response) => {
    return response;
  },
  async (error) => {
    if (error.response.status === 401) {
      await refreshAccessTokenFn();
      queryClient.setQueryData(["authUser"], await getCurrUser());
    }
  }
);

export const toURLSearchParams = (record: Record<string, unknown>) =>
  new URLSearchParams(
    Object.entries(record).reduce<string[][]>((result, [key, value]) => {
      if (Array.isArray(value)) {
        value.forEach((element) => result.push([key, String(element)]));
      } else {
        result.push([key, String(value)]);
      }
      return result;
    }, [])
  );
