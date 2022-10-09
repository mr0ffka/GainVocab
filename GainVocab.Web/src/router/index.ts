import { createRouter, createWebHistory } from "vue-router";

export const router = createRouter({
    history: createWebHistory(),
    routes: [
        {
            path: "/",
            name: "Home",
            component: () => import("../views/Home.vue")
        },
        {
            path: "/signin",
            name: "SignIn",
            component: () => import("../views/auth/SignIn.vue")
        },
        {
            path: "/register",
            name: "Register",
            component: () => import("../views/auth/Register.vue")
        },
        {
            path: "/register",
            name: "Register",
            component: () => import("../views/auth/Register.vue")
        },
        {
            path: "/dashboard",
            name: "Dashboard",
            meta: {
                requresAuth: true
            },
            component: () => import("../views/Home.vue"),
            children: [
                {
                    path: "test",
                    name: "Test",
                    component: () => import("../views/auth/Register.vue")
                }
            ]
        },
        {
            path: '/:pathMatch(.*)*',
            component: () => import("../views/PageNotFound.vue")
        },
    ]
})