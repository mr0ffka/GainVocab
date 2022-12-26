import { DateTime } from "luxon";
import { ICourseDataExampleAddModel } from "../admin/types";

export interface ISupportApplicationIssueModel {
  typePublicId: string;
  reporterId: string;
  issueMessage: string;
}

export interface ISupportCourseDataIssueModel {
  typePublicId: string;
  reporterId: string;
  issueMessage: string;
  issueEntityId: string;
}

export interface IUserProfileEditModel {
  firstName: string;
  lastName: string;
  currentPassword: string;
  password: string;
  passwordConfirm: string;
  courses: string[];
}

export interface IAddUserToCourseModel {
  userId: string;
  coursePublicId: string;
}

export interface ILearnCourseGetModel {
  userId: string;
  coursePublicId: string;
}

export interface ILearnCourseModel {
  currentDataPublicId: string;
  userCoursePublicId: string;
  name: string;
  languageFrom: string;
  languageTo: string;
  source: string;
  percentProgress: number;
  isFinished: boolean;
}

export interface ILearnCourseSendModel {
  userCoursePublicId: string;
  source: string;
  translation: string;
}

export interface ILearnCourseCheckModel {
  userId: string;
  userCoursePublicId: string;
  translation: string;
}

export interface ILearnCourseCheckResponseModel {
  isFinished: boolean;
  isError: boolean;
  wordIndexError?: number;
  percentProgress: number;
  hasExamples: boolean;
  examples: ICourseDataExampleAddModel[];
}

export interface ILearnCourseNextModel {
  userId: string;
  userCoursePublicId: string;
}

export interface ILearnCourseNextResponseModel {
  currentDataPublicId: string;
  source: string;
}
