import axios from "axios";

import type {
  GenericResponse,
  IResetPasswordModel,
  ILoginModel,
  ILoginResponse,
  IOAuthLoginModel,
  IRegisterModel,
  IUserAuth,
  IUserAuthResponse,
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

export const registerUserFn = async (user: IRegisterModel) => {
  return (await authApi.post<IUserAuthResponse>("auth/register", user)).data;
};

export const loginUserFn = async (user: ILoginModel) => {
  return (await authApi.post<IUserAuthResponse>("auth/login", user)).data;
};

export const oAuthGoogleLoginFn = async (oauth: IOAuthLoginModel) => {
  return (await authApi.post<IUserAuthResponse>("auth/googleLogin", oauth))
    .data;
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
  return (await authApi.get<IUserAuth>("auth/me")).data;
};
