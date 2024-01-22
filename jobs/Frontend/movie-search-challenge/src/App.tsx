import { ThemeProvider } from "styled-components"
import { theme } from "./styles/Theme"
import { GlobalStyles } from "./styles/Global"
import Header from "./components/header/Header"
import MovieSearch from "./pages/MovieSearch"

const App = () => {
  return (
    <ThemeProvider theme={theme}>
      <GlobalStyles />
      <Header />
      <MovieSearch />
    </ThemeProvider>
  )
}

export default App
