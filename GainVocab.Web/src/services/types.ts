export interface GenericResponse {
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

export interface ILoginResponse {
    status: string;
    access_token: string;
}

export interface IRegisterModel {
    name: string;
    email: string;
    password: string;
    password_confirm: string;
}

export interface IRegisterResponse {
    status: string;
    message: string;
}