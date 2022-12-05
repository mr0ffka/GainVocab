import { IUserAuth } from "../auth/types";

export interface IFilterModel {
  firstName: string;
  lastName: string;
}

export interface IUserModel {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
}
