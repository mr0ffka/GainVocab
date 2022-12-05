import { IUserAuth } from "@/services/auth/types";
import { getCurrUser } from "@/services/auth/authApi";
import { queryClient } from "@/helpers/queryClient";
import type { NavigationGuardNext } from "vue-router";

export default async function requireAuthMiddleware({
  next,
}: {
  next: NavigationGuardNext;
}) {
  try {
    const authResult = queryClient.getQueryData(["authUser"]) as IUserAuth;
    if (!authResult) {
      return next({
        name: "login",
      });
    }
  } catch (error) {
    return next({
      name: "error-500",
    });
  }

  return next();
}
