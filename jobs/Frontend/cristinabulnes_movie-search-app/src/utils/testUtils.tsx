import React from "react";
import { render, RenderOptions } from "@testing-library/react";
import { ThemeProvider } from "styled-components";
import { theme } from "../theme";

// Custom render function that wraps components with ThemeProvider
export const customRender = (
	ui: React.ReactElement,
	options?: RenderOptions
) => {
	return render(<ThemeProvider theme={theme}>{ui}</ThemeProvider>, options);
};

// Re-export everything from RTL (React Testing Library) to make it easier to use
export * from "@testing-library/react";
