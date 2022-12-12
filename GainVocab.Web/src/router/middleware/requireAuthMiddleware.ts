import { IUserAuth } from "@/services/auth/types";
import { queryClient } from "@/helpers/queryClient";
import type { NavigationGuardNext } from "vue-router";
import { getCurrUser, refreshAccessTokenFn } from "@/services/auth/authApi";

export default async function requireAuthMiddleware({
  next,
}: {
  next: NavigationGuardNext;
}) {
  try {
    let authResult = (await queryClient.getQueryData([
      "authUser",
    ])) as IUserAuth;
    if (!authResult || !authResult.isAuthenticated) {
      await refreshAccessTokenFn();
      authResult = await getCurrUser();
      queryClient.setQueryData(["authUser"], authResult);
      if (!authResult || !authResult.isAuthenticated) {
        return next({
          name: "login",
        });
      }
    }
  } catch (error) {
    return next({
      name: "error-500",
    });
  }

  return next();
}
