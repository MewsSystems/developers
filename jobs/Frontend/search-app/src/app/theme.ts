import { createGlobalStyle } from "styled-components";

export const darkTheme = {
  textColorPrimary: "#fff",
  textColorSecondary: "rgba(255, 255, 255, 0.7)",
  highlighted: "rgb(250, 208, 0);",
  backgroundColorPlatfrom: "#000",
  backgroundColorPrimary: "#151515",
  backgroundColorSecondary: "rgba(138, 153, 168, 0.1)",
  backgroundColorHover: "rgba(138, 153, 168, 0.25)",
};

export const GlobalStyle = createGlobalStyle`
  body {
    background-color: ${darkTheme.backgroundColorPlatfrom};
  }
`;
