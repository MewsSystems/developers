import { BrowserRouter, Route, Routes } from 'react-router-dom'

import { QueryClient, QueryClientProvider } from '@tanstack/react-query'

import { CssBaseline, ThemeProvider } from '@mui/material'

import { ErrorBoundary } from '../ErrorBoundary.tsx'
import theme from './styles/theme.tsx'

import Layout from '@/layout/Layout.tsx'
import HomePage from '@/pages/HomePage.tsx'
import MovieDetailPage from '@/pages/MovieDetailPage.tsx'
import NotFoundPage from '@/pages/NotFoundPage.tsx'
import { paths } from '@/paths.ts'

const queryClient = new QueryClient()

const App = () => (
    <ThemeProvider theme={theme}>
        <CssBaseline />
        <ErrorBoundary>
            <QueryClientProvider client={queryClient}>
                <BrowserRouter>
                    <Routes>
                        <Route path={paths.homepage} element={<Layout />}>
                            <Route path="" element={<HomePage />} />
                            <Route
                                path={`${paths.movieDetail}:id`}
                                element={<MovieDetailPage />}
                            />
                            <Route path="/*" element={<NotFoundPage />} />
                        </Route>
                    </Routes>
                </BrowserRouter>
            </QueryClientProvider>
        </ErrorBoundary>
    </ThemeProvider>
)

export default App
