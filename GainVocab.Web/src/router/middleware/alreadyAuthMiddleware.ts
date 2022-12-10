import { IUserAuth } from "@/services/auth/types";
import { getCurrUser } from "@/services/auth/authApi";
import { queryClient } from "@/helpers/queryClient";
import type { NavigationGuardNext, RouteLocationNormalized } from "vue-router";
import type { MiddlewareContext } from "../middlewarePipeline";

export default async function alreadyAuthMiddleware({
  from,
  next,
  to,
}: MiddlewareContext) {
  try {
    let authResult = (await queryClient.getQueryData([
      "authUser",
    ])) as IUserAuth;
    if (!authResult) {
      authResult = await getCurrUser();
      queryClient.setQueryData(["authUser"], authResult);
    }
    if (authResult.isAuthenticated) {
      if (authResult.isAdmin) {
        return next({
          name: "admin-dashboard",
        });
      }

      return next({
        name: "user-dashboard",
      });
    }

    return next();
  } catch (error) {
    return next({
      name: "error-500",
    });
  }
}
