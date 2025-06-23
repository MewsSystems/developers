import { QueryClient, QueryClientProvider } from "@tanstack/react-query"
import { ReactQueryDevtools } from "@tanstack/react-query-devtools"
import { Route, Routes } from "react-router"
import { ThemeProvider } from "styled-components"
import { ROUTES } from "./constants/routes"
import { MovieDetailPage } from "./pages/MovieDetailPage"
import { NotFoundPage } from "./pages/NotFoundPage"
import { SearchPage } from "./pages/SearchPage"
import { GlobalStyles } from "./styles/GlobalStyles"
import { theme } from "./styles/theme"

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
        <Routes>
          <Route path={ROUTES.HOME} element={<SearchPage />} />
          <Route path={ROUTES.MOVIE_DETAIL} element={<MovieDetailPage />} />
          <Route path={ROUTES.NOT_FOUND} element={<NotFoundPage />} />
        </Routes>
        <ReactQueryDevtools initialIsOpen={false} />
      </ThemeProvider>
    </QueryClientProvider>
  )
}

export default App
