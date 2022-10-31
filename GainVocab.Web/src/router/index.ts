import {
    createRouter,
    createWebHistory,
    RouteRecordRaw,
    type NavigationGuardNext,
    type RouteLocationNormalized,
} from 'vue-router';
import middlewarePipeline from '@/router/middlewarePipeline';
import AdminRoutes from '@/views/modules/admin/routes';
import UserRoutes from '@/views/modules/user/routes';

const routes: Array<RouteRecordRaw> = [
    {
        path: '/error',
        redirect: '/error/404',
        children: [
            {
                path: '401',
                name: 'error-401',
                component: () => import("../views/errors/Page401.vue")
            },
            {
                path: '403',
                name: 'error-403',
                component: () => import("../views/errors/Page403.vue")
            },
            {
                path: '404',
                name: 'error-404',
                component: () => import("../views/errors/Page404.vue")
            },
            {
                path: '500',
                name: 'error-500',
                component: () => import("../views/errors/Page500.vue")
            },
            {
                path: '503',
                name: 'error-503',
                component: () => import("../views/errors/Page503.vue")
            }
        ]
    },
    {
        path: "/auth",
        children: [
            {
                name: "login",
                path: "login",
                component: () => import("../views/auth/LoginView.vue")
            },
            {
                name: "register",
                path: "register",
                component: () => import("../views/auth/RegisterView.vue")
            },
            {
                name: 'verifyemail',
                path: 'verifyemail',
                component: () => import("../views/auth/VerifyEmailView.vue"),
                children: [
                    {
                        name: 'verifyemail',
                        path: ':verificationCode',
                        component: () => import("../views/auth/VerifyEmailView.vue")
                    },
                ],
            },
        ]
    },
    {
        path: "/",
        children: [
            {
                name: "home",
                path: "",
                component: () => import("../views/Home.vue")
            },
            ...AdminRoutes,
            ...UserRoutes
        ]
    },
    {
        path: '/:pathMatch(.*)*',
        redirect: '/error/404'
    },
];

const router = createRouter({
    history: createWebHistory('/'),
    routes,
});

router.beforeEach(
    (
        to: RouteLocationNormalized,
        from: RouteLocationNormalized,
        next: NavigationGuardNext
    ) => {
        
        if (!to.meta.middleware) {
            return next();
        }
        const middleware = to.meta.middleware as any;

        const context = {
            to,
            from,
            next
        };
        console.log("beforeEach");
        return middleware[0]({
            ...context,
            next: middlewarePipeline(context, middleware, 1),
        });
    }
);

export default router;
