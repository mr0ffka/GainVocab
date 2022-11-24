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
    ],
  },
];
