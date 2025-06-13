import {QueryClient, QueryClientProvider} from "@tanstack/react-query"
import {BrowserRouter, Route, Routes} from "react-router-dom"
import {SearchView} from "./views/SearchView.tsx"
import {MovieDetailView} from "./views/MovieDetailView.tsx"
import {GlobalStyles} from "./styles/GlobalStyles.tsx";
import {PageNotFound} from "./views/PageNotFound.tsx";
import {ThemeProvider} from "styled-components";
import {breakpoints, colors, darkColors, fontSizes, layout, radii, shadows, spacing} from "./styles/designTokens.ts";
import {useColorMode} from "./hooks/useColorMode.ts";
import {ThemeToggle} from "./components/ThemeToggle.tsx";

function App() {
    const queryClient = new QueryClient()
    const [theme, toggleTheme] = useColorMode();

    const themeObject = {
        colors: theme === 'dark' ? darkColors : colors,
        spacing,
        fontSizes: fontSizes,
        radii: radii,
        shadows: shadows,
        layout: layout,
        breakpoints: breakpoints
    };

    return (
        <>
            <QueryClientProvider client={queryClient}>
                <ThemeProvider theme={themeObject} >
                    <GlobalStyles/>
                    <ThemeToggle theme={theme} toggleTheme={toggleTheme}/>
                    <BrowserRouter>
                        <Routes>
                            <Route path="/" element={<SearchView/>}/>
                            <Route path="/movie/:id" element={<MovieDetailView/>}/>
                            <Route path="*" element={<PageNotFound/>}/>
                        </Routes>
                    </BrowserRouter>
                </ThemeProvider>
            </QueryClientProvider>
        </>
    )
}

export default App
