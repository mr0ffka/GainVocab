import { getCurrUser } from '@/services/authApi';
import { IUser } from '@/services/types';
import { queryClient } from '@/helpers/queryClient';
import type { NavigationGuardNext } from 'vue-router';

export default async function isAdminMiddleware({
    next,
}: {
    next: NavigationGuardNext;
}) {
    try {
        const authResult = queryClient.getQueryData(['authUser']) as IUser;
        if (!authResult) {
            const response = await getCurrUser();
            if (response.isAdmin) {
                queryClient.setQueryData(['authUser'], response);
            }
            else {
                if (!response.isAuthenticated) {
                    return next({
                        name: 'login',
                    })
                } else {
                    return next({
                        name: 'error-403',
                    })
                }
            }
        }
    } catch (error) {
        return next({
            name: 'error-500',
        })
    }

    return next();
}