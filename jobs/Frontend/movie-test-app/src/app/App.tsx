import { AppProvider } from './Provider.tsx';
import { AppRouter } from './Router.tsx';
import { GlobalStyle } from '../global.styles..ts';
import Header from '../components/header/index.tsx';

export const App = () => {
  return (
    <>
      <GlobalStyle />
      <AppProvider>
        <Header />
        <AppRouter />
      </AppProvider>
    </>
  );
};

export default App;
