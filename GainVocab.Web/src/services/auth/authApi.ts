import axios from "axios";
import { GenericResponse } from "../common/types";

import type {
  IResetPasswordModel,
  ILoginModel,
  ILoginResponse,
  IOAuthLoginModel,
  IRegisterModel,
  IUserAuth,
  IUserAuthResponse,
  IForgotPasswordModel,
} from "./types";

import { api } from "@/helpers/axios";

export const refreshAccessTokenFn = async () => {
  const response = await api.post("auth/refresh");
  return response.data;
};

export const registerUserFn = async (user: IRegisterModel) => {
  return (await api.post<IUserAuthResponse>("auth/register", user)).data;
};

export const loginUserFn = async (user: ILoginModel) => {
  const request = await api.post("auth/login", user);
  console.log(request);
  return request.data;
};

export const verifyEmailFn = async (
  userId: string,
  verificationCode: string
) => {
  const response = await api.get<GenericResponse>("auth/verifyemail", {
    params: {
      userId: userId,
      code: verificationCode,
    },
  });
  return response.data;
};

export const resetPasswordFn = async (form: IResetPasswordModel) => {
  return (await api.post<GenericResponse>("auth/resetpassword", form)).data;
};

export const forgotPasswordFn = async (model: IForgotPasswordModel) => {
  return (await api.post<GenericResponse>("auth/forgotpassword", model)).data;
};

export const logoutUserFn = async () => {
  const response = await api.get<GenericResponse>("auth/logout");
  return response.data;
};

export const getCurrUser = async () => {
  return (await api.get<IUserAuth>("auth/me")).data;
};
