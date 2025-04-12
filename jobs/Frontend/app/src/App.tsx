import styled, { createGlobalStyle } from 'styled-components';
import Providers from './providers';
import AppRouter from './routers';

const GlobalStyle = createGlobalStyle`
  body {
    margin: 0;
    font-family: 'Segoe UI', sans-serif;
    background-color: #f5f5f5;
  }
`;

const AppWrapper = styled.div`
  min-height: 100vh;
  display: flex;
  flex-direction: column;
`;

function App() {
  return (
    <Providers>
      <GlobalStyle />
      <AppWrapper>
        <AppRouter />
      </AppWrapper>
    </Providers>
  );
}

export default App;
