import { createBrowserRouter, RouteObject } from "react-router-dom";
import ErrorPage from "./components/ErrorPage/ErrorPage";
import { getDefaultLayout } from "./components/layout";
import Home from "./pages/Home/Home";

export const routerObjects: RouteObject[] = [
  {
    path: "/",
    Component: Home,
    children: [
      {
        path: "/:movieId",
        Component: Home,
      },
    ],
  },
];

export function createRouter(): ReturnType<typeof createBrowserRouter> {
  const routeWrappers = routerObjects.map((router) => {
    // @ts-ignore TODO: better type support
    const getLayout = router.Component?.getLayout || getDefaultLayout;
    const Component = router.Component!;
    const page = getLayout(<Component />);
    return {
      ...router,
      element: page,
      Component: null,
      ErrorBoundary: ErrorPage,
    };
  });
  return createBrowserRouter(routeWrappers);
}
