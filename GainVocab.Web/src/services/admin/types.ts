import { DateTime } from "luxon";

export interface IUserFilterModel {
  firstName: string;
  lastName: string;
  roles: [];
  courses: [];
}

export interface IUserModel {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  roles: string[];
  courses: string[];
}

export interface IUserAddModel {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  passwordConfirm: string;
  roles: string[];
  courses: string[];
}

export interface IUserDetailsModel {
  firstName: string;
  lastName: string;
  email: string;
  roles: string[];
  courses: [];
}

export interface IUserOptionModel {
  id: string;
  email: string;
}

export interface ILanguageAddModel {
  name: string;
}

export interface ILanguageModel {
  id: string;
  name: string;
}

export interface ILanguageListModel {
  id: string;
  name: string;
  courses: [];
}

export interface ILanguageFilterModel {
  name: string;
  courses: [];
}

export interface ICourseAddModel {
  name: string;
  languageFrom: string;
  languageTo: string;
}

export interface ICourseItemModel {
  id: string;
  name: string;
}

export interface ICourseModel {
  id: string;
  name: string;
  languageFrom: ILanguageModel;
  languageTo: ILanguageModel;
}

export interface ICourseListModel {
  id: string;
  name: string;
  languageFrom: ILanguageModel;
  languageTo: ILanguageModel;
  studentAmnt: number;
}

export interface ICourseFilterModel {
  name: string;
  languageFrom: string;
  languageTo: string;
}

export interface ICourseDataAddModel {
  source: string;
  translation: string;
  coursePublicId: string;
}

export interface ICourseDataUpdateModel {
  source: string;
  translation: string;
}

export interface ICourseDataFilterModel {
  publicId: string;
  source: string;
  translation: string;
}

export interface ICourseDataListModel {
  publicId: string;
  source: string;
  translation: string;
}

export interface ISupportIssueFilterModel {
  isResolved: string[];
  typeId: string[];
  reporterId: string[];
  createdFrom: string;
  createdTo: string;
  updatedFrom: string;
  updatedTo: string;
}

export interface IIssueEntityListItemModel {
  courseName: string;
  languageFrom: string;
  languageTo: string;
  source: string;
  translation: string;
}

export interface ISupportIssueListItemModel {
  id: string;
  typeName: string;
  isResolved: boolean;
  reporter: IUserDetailsModel;
  issueEntity: IIssueEntityListItemModel;
  message: string;
  createdAt: DateTime;
  updatedAt: DateTime;
}

export interface ISupportIssueTypeOptionModel {
  publicId: string;
  name: string;
}
