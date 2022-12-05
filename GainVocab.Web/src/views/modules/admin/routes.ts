import isAdminMiddleware from "@/router/middleware/isAdminMiddleware";

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
        meta: {
          middleware: [isAdminMiddleware],
        },
        children: [
          {
            path: "add",
            name: "user-add",
            component: () => import("@/views/modules/admin/user/UserForm.vue"),
          },
          {
            path: "edit/:id",
            name: "user-edit",
            component: () => import("@/views/modules/admin/user/UserForm.vue"),
          },
          {
            path: "details/:id",
            name: "user-details",
            component: () =>
              import("@/views/modules/admin/user/UserDetails.vue"),
          },
          {
            path: "",
            name: "user-list",
            component: () => import("@/views/modules/admin/user/UserList.vue"),
          },
        ],
      },
      {
        path: "support",
        meta: {
          middleware: [isAdminMiddleware],
        },
        children: [
          {
            path: "",
            name: "support-list",
            component: () =>
              import("@/views/modules/admin/support/SupportList.vue"),
          },
        ],
      },
      {
        path: "languages",
        meta: {
          middleware: [isAdminMiddleware],
        },
        children: [
          {
            path: "",
            name: "language-list",
            component: () =>
              import("@/views/modules/admin/language/LanguageList.vue"),
          },
          {
            path: "import",
            name: "language-import",
            component: () =>
              import("@/views/modules/admin/language/LanguageImport.vue"),
          },
        ],
      },
    ],
  },
];
