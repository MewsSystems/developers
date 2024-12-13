import { ThemeProvider } from "styled-components";
import MainLayout from "@/layouts/Main";
import Routing from "@/providers/Routing";
import theme from "@/styles/theme";
import GlobalStyles from "@/styles/global";
import ErrorCatcher from "@/providers/ErrorCatcher";
import ErrorFlash from "@/components/ErrorFlash/ErrorFlash";

function App() {
  return (
    <ThemeProvider theme={theme}>
      <ErrorCatcher>
        <ErrorFlash />
        <GlobalStyles />
        <MainLayout>
          <Routing />
        </MainLayout>
      </ErrorCatcher>
    </ThemeProvider>
  );
}

export default App;
