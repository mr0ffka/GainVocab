import type { RouteLocationNormalized, NavigationGuardNext } from "vue-router";

export type Middleware = (context: MiddlewareContext) => void

export type MiddlewareContext = {
  to: RouteLocationNormalized,
  from: RouteLocationNormalized,
  next: NavigationGuardNext,
}

function middlewarePipeline(context: MiddlewareContext, middleware: Middleware[], index: number) {
  const nextMiddleware = middleware[index];

  if (!nextMiddleware) {
    return context.next;
  }

  return () => {
    const nextPipeline = middlewarePipeline(context, middleware, index + 1);

    nextMiddleware({ ...context, next: nextPipeline });
  };
}

export default middlewarePipeline;
