import { IUser } from "@/services/types";
import { getCurrUser } from "@/services/authApi";
import { queryClient } from "@/helpers/queryClient";
import type { NavigationGuardNext, RouteLocationNormalized } from "vue-router";
import type { MiddlewareContext } from "../middlewarePipeline";

export default async function alreadyAuthMiddleware({
  from,
  next,
  to,
}: MiddlewareContext) {
  try {
    console.log("form: ", from, " to: ", to);
    let authResult = queryClient.getQueryData(["authUser"]) as IUser;
    if (!authResult) {
      console.log("dupa alreadyAuthMiddleware");
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
