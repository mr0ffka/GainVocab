import { getCurrUser, refreshAccessTokenFn } from "@/services/auth/authApi";
import {
  createRouter,
  createWebHistory,
  RouteRecordRaw,
  type NavigationGuardNext,
  type RouteLocationNormalized,
} from "vue-router";
import middlewarePipeline, {
  type Middleware,
} from "@/router/middlewarePipeline";
import AdminRoutes from "@/views/modules/admin/routes";
import UserRoutes from "@/views/modules/user/routes";
import alreadyAuthMiddleware from "./middleware/alreadyAuthMiddleware";
import { queryClient } from "@/helpers/queryClient";

declare module "vue-router" {
  interface RouteMeta {
    middleware?: Middleware[];
  }
}

const routes: Array<RouteRecordRaw> = [
  {
    path: "/error",
    redirect: "/error/404",
    children: [
      {
        path: "401",
        name: "error-401",
        component: () => import("../views/errors/Page401.vue"),
      },
      {
        path: "403",
        name: "error-403",
        component: () => import("../views/errors/Page403.vue"),
      },
      {
        path: "404",
        name: "error-404",
        component: () => import("../views/errors/Page404.vue"),
      },
      {
        path: "500",
        name: "error-500",
        component: () => import("../views/errors/Page500.vue"),
      },
      {
        path: "503",
        name: "error-503",
        component: () => import("../views/errors/Page503.vue"),
      },
    ],
  },
  {
    path: "/auth",
    children: [
      {
        name: "login",
        path: "login",
        meta: {
          middleware: [alreadyAuthMiddleware],
        },
        component: () => import("../views/auth/LoginView.vue"),
      },
      {
        name: "register",
        path: "register",
        meta: {
          middleware: [alreadyAuthMiddleware],
        },
        component: () => import("../views/auth/RegisterView.vue"),
      },
      {
        name: "verifyemail",
        path: "verifyemail",
        component: () => import("../views/auth/VerifyEmailView.vue"),
      },
      {
        name: "resetpassword",
        path: "resetpassword",
        component: () => import("../views/auth/ResetPasswordView.vue"),
      },
      {
        name: "forgotpassword",
        path: "forgotpassword",
        meta: {
          middleware: [alreadyAuthMiddleware],
        },
        component: () => import("../views/auth/ForgotPasswordView.vue"),
      },
    ],
  },
  {
    path: "/",
    children: [
      {
        name: "home",
        path: "",
        component: () => import("../views/Home.vue"),
      },
      ...AdminRoutes,
      ...UserRoutes,
    ],
  },
  {
    path: "/:pathMatch(.*)*",
    redirect: "/error/404",
  },
];

export const router = createRouter({
  history: createWebHistory("/"),
  routes,
});

router.beforeEach(
  async (
    to: RouteLocationNormalized,
    from: RouteLocationNormalized,
    next: NavigationGuardNext
  ) => {
    if (!queryClient.getQueryData(["authUser"])) {
      queryClient.invalidateQueries(["authUser"]);
    }

    if (!to.meta.middleware) {
      return next();
    }
    const middleware = to.meta.middleware;

    const context = {
      to,
      from,
      next,
    };

    return middleware[0]({
      ...context,
      next: middlewarePipeline(context, middleware, 1),
    });
  }
);

export default router;
