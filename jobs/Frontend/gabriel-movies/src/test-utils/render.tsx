import type { ReactNode } from "react";
import type { RenderOptions } from "@testing-library/react";
import { render as rtlRender } from "@testing-library/react";
import { TestProviders } from "./TestProviders";
import type { TestProvidersProps } from "./TestProviders";

type TestRenderOptions = RenderOptions & Omit<TestProvidersProps, "children">;

export function render(ui: ReactNode, options?: TestRenderOptions) {
  const { initialEntries, ...rest } = options ?? {};
  const Wrapper = ({ children }: { children: ReactNode }) => {
    return <TestProviders initialEntries={initialEntries}>{children}</TestProviders>
  };
  return rtlRender(ui, { wrapper: Wrapper, ...rest });
}
