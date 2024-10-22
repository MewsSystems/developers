import { AppProvider } from './Provider.tsx';
import { AppRouter } from './Router.tsx';

export const App = () => {
  return (
    <>
      <h1>App</h1>
      <AppProvider>
        <AppRouter />
      </AppProvider>
    </>
  );
};

export default App;
