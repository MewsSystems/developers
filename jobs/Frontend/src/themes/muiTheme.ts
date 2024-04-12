import { createTheme } from '@mui/material'

const rootElement = document.getElementById('root')

export const muiTheme = createTheme({
    typography: {
        fontSize: 16,
        fontFamily: ['Noto Sans', 'sans-serif'].join(','),
        allVariants: {
            color: '#333',
        },
    },
    shape: {
        borderRadius: 6,
    },
    palette: {},
    components: {
        MuiPopover: {
            defaultProps: {
                container: rootElement,
            },
        },
        MuiPopper: {
            defaultProps: {
                container: rootElement,
            },
        },
        MuiDialog: {
            defaultProps: {
                container: rootElement,
            },
        },
        MuiModal: {
            defaultProps: {
                container: rootElement,
            },
        },
        MuiMenu: {
            defaultProps: {
                container: rootElement,
            },
        },
    },
})
