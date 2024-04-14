import { createTheme } from '@mui/material'

const rootElement = document.getElementById('root')

export const muiTheme = createTheme({
    typography: {
        fontSize: 16,
        fontFamily: ['Kanit', 'sans-serif'].join(','),
        allVariants: {
            color: '#333',
        },
    },
    shape: {
        borderRadius: 6,
    },
    palette: {
        primary: {
            main: '#AADBE7',
            contrastText: '#525B69',
        },
    },
    components: {
        MuiButton: {
            styleOverrides: {
                contained: {
                    boxShadow: 'none',
                },
            },
        },
    },
})
