import { CssBaseline, Stack } from '@mui/material';
import { ThemeProvider } from '@mui/material/styles';
import Search from './features/search/components/Search';
import { theme } from './themes/theme';

function App() {
  return (
    <ThemeProvider theme={theme}>
      <CssBaseline>
        <Stack
          sx={{
            marginTop: 'calc(100svh / 32)'
          }}
          direction="column"
          alignItems="center"
          justifyContent="center">
          <Search></Search>
        </Stack>
      </CssBaseline>
    </ThemeProvider>
  );
}

export default App;
