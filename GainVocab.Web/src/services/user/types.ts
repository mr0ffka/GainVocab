import { IUserAuth } from "../auth/types";

export type UserRole = typeof USER_ROLES[number];
export const USER_ROLES = ["Administrator", "User"] as const;

export type UserRole2 = "Administrator" | "User";

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
  roles: UserRole2[];
}

export interface IUserAddModel {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  passwordConfirm: string;
  roles: UserRole2[];
}
