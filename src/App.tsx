import { QueryClient, QueryClientProvider } from "@tanstack/react-query"
import { ReactQueryDevtools } from "@tanstack/react-query-devtools"
import { lazy, Suspense } from "react"
import { Route, Routes } from "react-router"
import { ThemeProvider } from "styled-components"
import { PageLoader } from "@/components/PageLoader"
import { ROUTES } from "@/constants/routes"
import { GlobalStyles } from "@/styles/GlobalStyles"
import { theme } from "@/styles/theme"

const SearchPage = lazy(() => import("@/pages/SearchPage"))
const MovieDetailPage = lazy(() => import("@/pages/MovieDetailPage"))
const NotFoundPage = lazy(() => import("@/pages/NotFoundPage"))

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      retry: 1,
      refetchOnWindowFocus: false,
    },
  },
})

function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <ThemeProvider theme={theme}>
        <GlobalStyles />
        <Suspense fallback={<PageLoader />}>
          <Routes>
            <Route path={ROUTES.HOME} element={<SearchPage />} />
            <Route path={ROUTES.MOVIE_DETAIL} element={<MovieDetailPage />} />
            <Route path={ROUTES.NOT_FOUND} element={<NotFoundPage />} />
          </Routes>
        </Suspense>
        <ReactQueryDevtools initialIsOpen={false} />
      </ThemeProvider>
    </QueryClientProvider>
  )
}

export default App
