import { createTheme } from "@mui/material/styles";

const theme = createTheme({
  palette: {
    primary: {
      main: "#7589F3",
    },
    secondary: {
      main: "#EDC239",
    },
  },
  typography: {
    fontSize: 16,
    h1: { fontSize: 56, fontWeight: 700 },
    h2: { fontSize: 32, fontWeight: 700 },
    h3: { fontSize: 20, fontWeight: 700 },
  },

  components: {
    MuiTypography: {
      styleOverrides: {
        root: {
          color: "#F1F1E6",
        },
      },
    },
    MuiFormHelperText: {
      styleOverrides: {
        root: {
          color: "#F1F1E6",
        },
      },
    },
    MuiOutlinedInput: {
      styleOverrides: {
        root: {
          borderRadius: 50,
          backgroundColor: "#0d0e16",
          color: "#F1F1E6",
        },
      },
    },
    MuiPaginationItem: {
      styleOverrides: {
        root: {
          color: "#F1F1E6",
        },
      },
    },
  },
});

export default theme;
