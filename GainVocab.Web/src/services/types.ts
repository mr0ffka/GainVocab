export interface GenericResponse {
    succeeded: any;
    status: string;
    message: string;
}

export interface IUser {
    isAuthenticated: boolean;
    isAdmin: boolean;
    email: string;
    roles: [];
    id: string;
}

export interface IUserResponse {
    status: string;
    user: IUser;
}

export interface ILoginModel {
    email: string;
    password: string;
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