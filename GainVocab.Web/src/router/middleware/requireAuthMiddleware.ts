import { IUser } from '@/services/types';
import { getCurrUser } from '@/services/authApi';
import { queryClient } from '@/helpers/queryClient';
import type { NavigationGuardNext } from 'vue-router';

export default async function requireAuthMiddleware({
  next,
}: {
  next: NavigationGuardNext;
}) {
  try {
    const authResult = queryClient.getQueryData(['authUser']) as IUser;
    if (!authResult) {
      const response = await getCurrUser();
      if (response.isAuthenticated) {
        queryClient.setQueryData(['authUser'], response);
      }
      else {
        return next({
          name: 'login',
        })
      }
    }
  } catch (error) {
    return next({
      name: 'error-500',
    })
  }

  return next();
}
