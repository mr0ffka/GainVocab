import { DateTime } from "luxon";

export interface ISupportApplicationIssueModel {
  typePublicId: string;
  reporterId: string;
  issueMessage: string;
}

export interface IUserProfileEditModel {
  firstName: string;
  lastName: string;
  currentPassword: string;
  password: string;
  passwordConfirm: string;
}