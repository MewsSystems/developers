import { ThemeProvider } from 'styled-components';
import { RoutesConfig } from './routes';

import { darkTheme, lightTheme } from './styles/themes';
import { GlobalStyle } from './styles/Globalstyle';
import { Header } from './components';
import { useThemeStore } from './store/themeStore';
import { LIGHT_THEME } from './constants';
import { ScrollToTop } from './components/ScrollToTop/ScrollToTop';

function App() {
  const theme = useThemeStore(state => state.theme);
  const currentTheme = theme === LIGHT_THEME ? lightTheme : darkTheme;

  return (
    <ThemeProvider theme={currentTheme}>
      <ScrollToTop />
      <GlobalStyle />
      <Header />
      <RoutesConfig />
    </ThemeProvider>
  );
}

export default App;
