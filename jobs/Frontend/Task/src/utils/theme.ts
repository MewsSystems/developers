import { createTheme } from "@mui/material";

const typography = {};

const palette = {
  // primary: { main: "#eae0d5", contrastText: "#5e503f"},
  // secondary: { main: "#3d405b", contrastText: "#eae0d5"},
  // background: { paper: "#eae0d5"}
  primary: { main: "#D3E7EB", contrastText: "#2C2C2C"},
  secondary: { main: "#030A45", contrastText: "#eae0d5"},
  background: { paper: "#fff"}
};

export const theme = createTheme({
  typography,
  palette
});
