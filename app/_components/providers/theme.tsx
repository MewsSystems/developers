"use client";

import { FC, ReactNode } from "react";
import { ThemeProvider as SCThemeProvider } from "styled-components";
import StyledComponentsRegistry from "@/lib/registry";
import { theme } from "@/styles/theme";
import { GlobalStyles } from "@/styles/global";

type Props = {
  children: ReactNode;
};

export const ThemeProvider: FC<Props> = ({ children }: Props) => {
  return (
    <StyledComponentsRegistry>
      <SCThemeProvider theme={theme}>
        <GlobalStyles />
        {children}
      </SCThemeProvider>
    </StyledComponentsRegistry>
  );
};
