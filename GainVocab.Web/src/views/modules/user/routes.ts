import requireAuth from "@/router/middleware/requireAuthMiddleware";

export default [
  {
    path: "/",
    name: "user-layout",
    meta: {
      middleware: [requireAuth],
    },
    children: [
      {
        path: "dashboard",
        name: "user-dashboard",
        meta: {
          middleware: [requireAuth],
        },
        component: () => import("@/views/modules/user/Dashboard.vue"),
      },
      {
        path: "profile",
        name: "user-profile",
        meta: {
          middleware: [requireAuth],
        },
        component: () => import("@/views/modules/user/UserProfile.vue"),
      },
      {
        path: "courses",
        name: "user-course-list",
        meta: {
          middleware: [requireAuth],
        },
        component: () => import("@/views/modules/user/CourseList.vue"),
      },
      {
        path: "learn/:id",
        name: "user-learn",
        meta: {
          middleware: [requireAuth],
        },
        component: () => import("@/views/modules/user/LearnVocab.vue"),
      },
    ],
  },
];
