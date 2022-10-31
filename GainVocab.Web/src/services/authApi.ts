import axios from 'axios';
import type {
    GenericResponse,
    ILoginModel,
    ILoginResponse,
    IRegisterModel,
    IUser,
    IUserResponse,
} from './types';

const authApi = axios.create({
    baseURL: import.meta.env.VITE_API_URL,
    withCredentials: true,
});

authApi.defaults.headers.common['Content-Type'] = 'application/json';

export const refreshAccessTokenFn = async () => {
    const response = await authApi.get<ILoginResponse>('auth/refresh');
    return response.data;
};

// authApi.interceptors.response.use(
//     (response) => {
//         return response;
//     },
//     async (error) => {
//         const originalRequest = error.config;
//         const errMessage = error.response.data.message as string;
//         if (errMessage.includes('not logged in') && !originalRequest._retry) {
//             originalRequest._retry = true;
//             await refreshAccessTokenFn();
//             return authApi(originalRequest);
//         }
//         return Promise.reject(error);
//     }
// );

export const signUpUserFn = async (user: IRegisterModel) => {
    const response = await authApi.post<GenericResponse>('auth/register', user);
    return response.data;
};

export const loginUserFn = async (user: ILoginModel) => {
    return (await authApi.post<IUserResponse>('auth/login', user)).data;
};

export const verifyEmailFn = async (verificationCode: string) => {
    const response = await authApi.get<GenericResponse>(
        `auth/verifyemail/${verificationCode}`
    );
    return response.data;
};

export const logoutUserFn = async () => {
    const response = await authApi.get<GenericResponse>('auth/logout');
    return response.data;
};

export const getCurrUser = async () => {
    return (await authApi.get<IUser>('users/me')).data;
};
