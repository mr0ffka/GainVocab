import { refreshAccessTokenFn } from "@/services/auth/authApi";
import axios from "axios";

export const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
  withCredentials: true,
});

api.defaults.headers.common["Content-Type"] = "application/json";

api.interceptors.response.use(
  async (response) => {
    if (response.status === 401) {
      await refreshAccessTokenFn();
    }
    return response;
  },
  (error) => {
    if (error.response && error.response.data) {
      return Promise.reject(error.response.data);
    }
    return Promise.reject(error.message);
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
