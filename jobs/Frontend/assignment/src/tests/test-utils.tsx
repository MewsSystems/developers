/* eslint-disable react-refresh/only-export-components */
import { PropsWithChildren, ReactElement } from "react";
import { render, RenderOptions } from "@testing-library/react";
import { ThemeProvider } from "styled-components";
import { materialLightTheme } from "../theme/themes";
import { BrowserRouter } from "react-router-dom";

function AllProviders({ children }: PropsWithChildren<unknown>) {
  return (
    <BrowserRouter>
      <ThemeProvider theme={materialLightTheme}>{children}</ThemeProvider>
    </BrowserRouter>
  );
}
const customRender = (ui: ReactElement, options?: RenderOptions) =>
  render(ui, { wrapper: AllProviders, ...options });

export * from "@testing-library/react";
export { customRender as render };
