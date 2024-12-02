import { CssBaseline, ThemeProvider } from '@mui/material'
import { muiTheme } from './themes'
import { AppRouting, CssPriorityProvider } from './providers'
import { BrowserRouter as Router } from 'react-router-dom'
import AppQueryProvider from './providers/AppQueryProvider'

function App() {
    return (
        <>
            <AppQueryProvider>
                <CssPriorityProvider>
                    <CssBaseline />
                    <ThemeProvider theme={muiTheme}>
                        <Router>
                            <AppRouting />
                        </Router>
                    </ThemeProvider>
                </CssPriorityProvider>
            </AppQueryProvider>
        </>
    )
}

export default App
