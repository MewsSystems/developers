import { createTheme } from "@mui/material/styles";

import { colors } from "@/styles/colors.ts";

const theme = createTheme({
  palette: {
    primary: {
      main: colors.primary.blue,
    },
    secondary: {
      main: colors.primary.yellow,
    },
  },
  typography: {
    fontSize: 16,
    h1: { fontSize: 56, fontWeight: 700 },
    h2: { fontSize: 32, fontWeight: 700 },
    h3: { fontSize: 20, fontWeight: 700 },
    body2: { fontSize: 14 },
  },

  components: {
    MuiDivider: {
      defaultProps: {
        "aria-hidden": true,
        color: colors.primary.yellow,
        flexItem: true,
      },
      styleOverrides: {
        root: {
          width: 5,
        },
      },
    },
    MuiFormHelperText: {
      styleOverrides: {
        root: {
          color: colors.primary.font,
        },
      },
    },
    MuiOutlinedInput: {
      styleOverrides: {
        root: {
          borderRadius: 50,
          backgroundColor: colors.secondary.darkBlue,
          color: colors.primary.font,
        },
      },
    },
    MuiPaginationItem: {
      styleOverrides: {
        root: {
          color: colors.primary.font,
        },
      },
    },
    MuiTypography: {
      styleOverrides: {
        root: {
          color: colors.primary.font,
        },
      },
    },
  },
});

export default theme;
