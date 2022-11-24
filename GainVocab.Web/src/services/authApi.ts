import axios from "axios";
import merge from "lodash/merge";

import type {
  GenericResponse,
  IResetPasswordModel,
  ILoginModel,
  ILoginResponse,
  IOAuthLoginModel,
  IRegisterModel,
  IUser,
  IUserResponse,
  IForgotPasswordModel,
} from "./types";

const authApi = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
  withCredentials: true,
});

authApi.defaults.headers.common["Content-Type"] = "application/json";

export const refreshAccessTokenFn = async () => {
  const response = await authApi.get<ILoginResponse>("auth/refresh");
  return response.data;
};

// authApi.interceptors.response.use(
//     (response) => {
//         return response;
//     },
//     async (error) => {
//         const originalRequest = error.config;
//         const errMessage = error.response.data.message as string;
//         if (errMessage.includes('not logged in') && !originalRequest._retry) {
//             originalRequest._retry = true;
//             await refreshAccessTokenFn();
//             return authApi(originalRequest);
//         }
//         return Promise.reject(error);
//     }
// );

export const registerUserFn = async (user: IRegisterModel) => {
  return (await authApi.post<IUserResponse>("auth/register", user)).data;
};

export const loginUserFn = async (user: ILoginModel) => {
  return (await authApi.post<IUserResponse>("auth/login", user)).data;
};

export const oAuthGoogleLoginFn = async (oauth: IOAuthLoginModel) => {
  return (await authApi.post<IUserResponse>("auth/googleLogin", oauth)).data;
};

export const verifyEmailFn = async (
  userId: string,
  verificationCode: string
) => {
  const response = await authApi.get<GenericResponse>("auth/verifyemail", {
    params: {
      userId: userId,
      code: verificationCode,
    },
  });
  return response.data;
};

export const resetPasswordFn = async (form: IResetPasswordModel) => {
  return (await authApi.post<GenericResponse>("auth/resetpassword", form)).data;
};

export const forgotPasswordFn = async (model: IForgotPasswordModel) => {
  return (await authApi.post<GenericResponse>("auth/forgotpassword", model))
    .data;
};

export const logoutUserFn = async () => {
  const response = await authApi.get<GenericResponse>("auth/logout");
  return response.data;
};

export const getCurrUser = async () => {
  return (await authApi.get<IUser>("users/me")).data;
};
