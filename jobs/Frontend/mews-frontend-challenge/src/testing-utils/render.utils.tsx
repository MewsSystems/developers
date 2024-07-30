import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import {
  Queries,
  render as renderInternal,
  RenderOptions,
  RenderResult,
} from "@testing-library/react";
import { MemoryRouter } from "react-router-dom";

export * from "@testing-library/react";

export type WidgetTestProps = {
  queryClient?: QueryClient;
  currentRoute?: string;
};

export function render<
  Q extends Queries,
  Container extends Element | DocumentFragment = HTMLElement,
  BaseElement extends Element | DocumentFragment = Container,
>(
  ui: React.ReactNode,
  {
    queryClient = new QueryClient(),
    currentRoute = "/",
    ...options
  }: RenderOptions<Q, Container, BaseElement> & WidgetTestProps = {},
): RenderResult<Q, Container, BaseElement> {
  return renderInternal(ui, {
    wrapper: ({ children }) => {
      return (
        <QueryClientProvider client={queryClient}>
          <MemoryRouter initialEntries={[currentRoute]}>
            {children}
          </MemoryRouter>
        </QueryClientProvider>
      );
    },
    ...options,
  });
}

export const noop = () => {};
