import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { SearchView } from './views/SearchView';
import { MovieDetailsView } from './views/MovieDetailsView';
import { MovieProvider } from './context/MovieContext';
import { ThemeProvider } from './context/ThemeContext';
import styled from 'styled-components';
import { ThemeToggle } from './components';

const AppContainer = styled.div.attrs({
  className: 'app-container',
})`
  min-height: 100vh;
  background-color: ${({ theme }) => theme.colors.background};
  color: ${({ theme }) => theme.colors.text.primary};
  transition:
    background-color 0.3s,
    color 0.3s;
  max-width: 1200px;
  margin: 0 auto;
  padding: ${({ theme }) => theme.spacing.xl};
`;

const TopBar = styled.div.attrs({
  className: 'top-bar',
})`
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: ${({ theme }) => theme.spacing.md};
`;

const Title = styled.h1.attrs({
  className: 'title',
})`
  font-size: 2rem;
  color: ${({ theme }) => theme.colors.text.primary};
  margin-bottom: ${({ theme }) => theme.spacing.md};
`;

function App() {
  return (
    // Provide a global theme context for the whole applicaton
    <ThemeProvider>
      {/* Provide movie search context and "caching" at a global level*/}
      <MovieProvider>
        {/* Main background styling common for both views */}
        <AppContainer>
          {/* Router for easy page navigation */}
          <BrowserRouter
            future={{
              v7_startTransition: true,
              v7_relativeSplatPath: true,
            }}
          >
            {/* Global top bar with title and theme toggle */}
            <TopBar>
              <Title>What to watch...</Title>
              <ThemeToggle />
            </TopBar>
            <Routes>
              <Route path="/search" element={<SearchView />} />
              <Route path="/movie/:id" element={<MovieDetailsView />} />
              {/* Redirect to search view if he introduces non matching route */}
              <Route path="/" element={<Navigate to="/search" replace />} />
            </Routes>
          </BrowserRouter>
        </AppContainer>
      </MovieProvider>
    </ThemeProvider>
  );
}

export default App;
