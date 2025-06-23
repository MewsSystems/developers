import {
  Outlet,
  ScrollRestoration,
  createRootRouteWithContext,
} from '@tanstack/react-router';
import { Meta, Scripts } from '@tanstack/start';
import type { ReactNode } from 'react';
import styles from '../styles/app.css?url';
import { QueryClient } from '@tanstack/react-query';

export interface RootContext {
  queryClient: QueryClient;
}

export const Route = createRootRouteWithContext<RootContext>()({
  head: () => ({
    meta: [
      {
        charSet: 'utf-8',
      },
      {
        name: 'viewport',
        content: 'width=device-width, initial-scale=1',
      },
      {
        title: 'Movies',
      },
    ],
    links: [{ rel: 'stylesheet', href: styles }],
  }),
  component: RootComponent,
});

function RootComponent() {
  return (
    <RootDocument>
      <Outlet />
    </RootDocument>
  );
}

function RootDocument({ children }: Readonly<{ children: ReactNode }>) {
  return (
    <html className="dark">
      <head>
        <Meta />
      </head>
      <body className="bg-white dark:bg-slate-800 dark:text-gray-300">
        {children}
        <ScrollRestoration />
        <Scripts />
      </body>
    </html>
  );
}
