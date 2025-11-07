import { ReactElement, isValidElement } from 'react';
import {
  RouteObject,
  RouterProvider,
  createMemoryRouter,
} from 'react-router-dom';
import { render, RenderOptions } from '@testing-library/react';
import { QueryClientProvider } from '@tanstack/react-query';
import queryClient from '@/utils/queryClientMock';

type ExtendedRenderOptions = RenderOptions & {
  routerHistory?: string[];
  routes?: RouteObject[];
};

const renderWithQueryClientAndRouter = (
  children: ReactElement | RouteObject,
  renderOptions?: ExtendedRenderOptions,
) => {
  const options = isValidElement(children)
    ? { element: children, path: '/' }
    : (children as RouteObject);

  const router = createMemoryRouter(
    [{ ...options }, ...(renderOptions?.routes ?? [])],
    {
      initialEntries: renderOptions?.routerHistory ?? ['/'],
      initialIndex: 1,
    },
  );

  return render(
    <QueryClientProvider client={queryClient}>
      <RouterProvider router={router} />
    </QueryClientProvider>,
  );
};

// eslint-disable-next-line react-refresh/only-export-components
export * from '@testing-library/react';
export { renderWithQueryClientAndRouter };
