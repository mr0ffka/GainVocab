import isAdminMiddleware from "@/router/middleware/isAdminMiddleware";
import requireAuthMiddleware from "@/router/middleware/requireAuthMiddleware";

export default [
  {
    path: "/admin",
    name: "admin",
    meta: {
      middleware: [isAdminMiddleware],
    },
    children: [
      {
        path: "dashboard",
        name: "admin-dashboard",
        meta: {
          middleware: [isAdminMiddleware],
        },
        component: () => import("@/views/modules/admin/Dashboard.vue"),
      },
      {
        path: "users",
        name: "users",
        meta: {
          middleware: [isAdminMiddleware],
        },
        component: () => import("@/views/modules/admin/user/UserList.vue"),
      },
      {
        path: "support",
        name: "support",
        meta: {
          middleware: [isAdminMiddleware],
        },
        component: () => import("@/views/modules/admin/support/Support.vue"),
      },
      {
        path: "languages",
        name: "languages",
        meta: {
          middleware: [isAdminMiddleware],
        },
        component: () =>
          import("@/views/modules/admin/language/LanguageList.vue"),
      },
      {
        path: "languages/import",
        name: "language-import",
        meta: {
          middleware: [isAdminMiddleware],
        },
        component: () =>
          import("@/views/modules/admin/language/LanguageImport.vue"),
      },
    ],
  },
];
