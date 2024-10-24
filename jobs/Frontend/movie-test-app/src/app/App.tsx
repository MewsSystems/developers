import { AppProvider } from './Provider.tsx';
import { AppRouter } from './Router.tsx';
import { GlobalStyle } from '../global.styles..ts';

export const App = () => {
  return (
    <>
      <GlobalStyle />
      <AppProvider>
        <AppRouter />
      </AppProvider>
    </>
  );
};

export default App;
