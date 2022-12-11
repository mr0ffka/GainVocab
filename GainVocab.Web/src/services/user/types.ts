import { IUserAuth } from "../auth/types";

export type UserRole = "Administrator" | "User";

export interface IFilterModel {
  firstName: string;
  lastName: string;
  roles: [];
}

export interface IUserModel {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  roles: UserRole[];
}

export interface IUserAddModel {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  passwordConfirm: string;
  roles: UserRole[];
}

export interface IUserDetailsModel {
  firstName: string;
  lastName: string;
  email: string;
  roles: UserRole[];
  courses: [];
}
