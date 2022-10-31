import isAdminMiddleware from '@/router/middleware/isAdminMiddleware';
import requireAuthMiddleware from '@/router/middleware/requireAuthMiddleware';

export default [
    {
        path: '/admin',
        name: 'admin',
        meta: {
            middleware: [
                isAdminMiddleware
            ],
        },
        children: [
            {
                path: 'dashboard',
                name: 'admin-dashboard',
                meta: {
                    middleware: [
                        isAdminMiddleware
                    ],
                },
                component: () => import('@/views/modules/admin/Dashboard.vue')
            },
        ]
    },
];