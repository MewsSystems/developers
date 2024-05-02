import { amber, blue, deepPurple, grey, red } from '@mui/material/colors';
import { createTheme } from '@mui/material/styles';

export const theme = createTheme({
  palette: {
    mode: 'light',
    primary: {
      main: deepPurple[500]
    },
    secondary: {
      main: grey[500]
    },
    error: {
      main: red[500]
    },
    warning: {
      main: amber[500]
    },
    info: {
      main: deepPurple[500]
    },
    success: {
      main: blue[500]
    },
    background: {
      default: '#f7f7f7'
    }
  },
  typography: {
    button: {
      textTransform: 'capitalize'
    }
  },
  components: {
    MuiButtonBase: {
      defaultProps: {
        disableRipple: true
      }
    },
    MuiChip: {
      styleOverrides: {
        root: {
          borderRadius: '4px'
        }
      }
    },
    MuiTab: {
      defaultProps: {
        disableRipple: true
      }
    },
    MuiTable: {
      defaultProps: {
        size: 'small'
      }
    }
  }
});
