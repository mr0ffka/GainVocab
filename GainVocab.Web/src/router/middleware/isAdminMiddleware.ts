import { getCurrUser, refreshAccessTokenFn } from "@/services/auth/authApi";
import { IUserAuth } from "@/services/auth/types";
import { queryClient } from "@/helpers/queryClient";
import type { NavigationGuardNext } from "vue-router";

export default async function isAdminMiddleware({
  next,
}: {
  next: NavigationGuardNext;
}) {
  try {
    let authResult = (await queryClient.getQueryData([
      "authUser",
    ])) as IUserAuth;
    if (!authResult) {
      authResult = await getCurrUser();
      queryClient.setQueryData(["authUser"], authResult);
    }
    if (authResult.isAdmin) {
      return next();
    } else if (!authResult.isAuthenticated) {
      await refreshAccessTokenFn();
      authResult = await getCurrUser();
      queryClient.setQueryData(["authUser"], authResult);
      if (authResult.isAdmin) {
        return next();
      } else {
        return next({
          name: "login",
        });
      }
    } else {
      return next({
        name: "error-403",
      });
    }
  } catch (error) {
    console.log(error);
    return next({
      name: "error-500",
    });
  }
}
