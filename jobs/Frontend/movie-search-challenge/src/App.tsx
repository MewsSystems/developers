import { ThemeProvider } from "styled-components"
import { theme } from "./styles/Theme"
import { GlobalStyles } from "./styles/Global"
import MovieSearch from "./pages/MovieSearch"
import { BrowserRouter, Routes, Route } from "react-router-dom"
import MovieInfo from "./pages/MovieInfo"
import NotFound from "./pages/NotFound"

const App = () => {
  return (
    <ThemeProvider theme={theme}>
      <BrowserRouter>
        <GlobalStyles />
        <Routes>
          <Route path="/" element={<MovieSearch />} />
          <Route path="/:id" element={<MovieInfo />} />
          <Route path="*" element={<NotFound />} />
        </Routes>
      </BrowserRouter>
    </ThemeProvider>
  )
}

export default App
