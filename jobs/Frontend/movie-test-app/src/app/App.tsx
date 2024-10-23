import { AppProvider } from './Provider.tsx';
import { AppRouter } from './Router.tsx';
import { GlobalStyle } from '../global.styles..ts';
import { Header, HeaderPlaceholder } from '../components/header/index.tsx';

export const App = () => {
  return (
    <>
      <GlobalStyle />
      <AppProvider>
        <Header />
        <HeaderPlaceholder /> {/* Placeholder for the header */}
        <AppRouter />
      </AppProvider>
    </>
  );
};

export default App;
