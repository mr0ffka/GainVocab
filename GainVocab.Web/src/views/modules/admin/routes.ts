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
              import("@/views/modules/admin/support/SupportIssueList.vue"),
          },
        ],
      },
      {
        path: "course",
        meta: {
          middleware: [isAdminMiddleware],
        },
        children: [
          {
            path: "language",
            children: [
              {
                path: "",
                name: "language-list",
                component: () =>
                  import("@/views/modules/admin/course/LanguageList.vue"),
              },
              {
                path: "add",
                name: "language-add",
                component: () =>
                  import("@/views/modules/admin/course/LanguageForm.vue"),
              },
            ],
          },
          {
            path: "",
            name: "course-list",
            component: () =>
              import("@/views/modules/admin/course/CourseList.vue"),
          },
          {
            path: "add",
            name: "course-add",
            component: () =>
              import("@/views/modules/admin/course/CourseForm.vue"),
          },
          {
            path: "data",
            children: [
              {
                path: "",
                name: "data-list",
                component: () =>
                  import("@/views/modules/admin/course/DataList.vue"),
              },
              {
                path: "add",
                name: "data-add",
                component: () =>
                  import("@/views/modules/admin/course/DataForm.vue"),
              },
              {
                path: "edit/:publicId",
                name: "data-edit",
                component: () =>
                  import("@/views/modules/admin/course/DataForm.vue"),
              },
              {
                path: "import",
                name: "data-import",
                component: () =>
                  import("@/views/modules/admin/course/DataImport.vue"),
              },
            ],
          },
        ],
      },
    ],
  },
];
