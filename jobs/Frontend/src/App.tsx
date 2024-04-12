import { CssBaseline, ThemeProvider } from '@mui/material'
import { muiTheme } from './themes'
import { AppRouting, CssPriorityProvider } from './providers'
import { BrowserRouter as Router } from 'react-router-dom'

function App() {
    return (
        <>
            <CssPriorityProvider>
                <CssBaseline />
                <ThemeProvider theme={muiTheme}>
                    <Router>
                        <AppRouting />
                    </Router>
                </ThemeProvider>
            </CssPriorityProvider>
        </>
    )
}

export default App
