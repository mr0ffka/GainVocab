export interface IUserAuth {
  isAuthenticated: boolean;
  isAdmin: boolean;
  email: string;
  roles: [];
  id: string;
  firstName: string;
  lastName: string;
}

export interface IUserAuthResponse {
  status: string;
  user: IUserAuth;
}

export interface ILoginModel {
  email: string;
  password: string;
  rememberMe: boolean;
}

export interface IOAuthLoginModel {
  token: string;
  provider: string;
}

export interface ILoginResponse {
  status: string;
  accessToken: string;
}

export interface IRegisterModel {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  passwordConfirm: string;
}

export interface IRegisterResponse {
  status: string;
  message: string;
}

export interface IForgotPasswordModel {
  email: string;
}

export interface IResetPasswordModel {
  userId: string;
  resetToken: string;
  newPassword: string;
  newPasswordConfirm: string;
}
