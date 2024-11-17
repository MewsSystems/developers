import React from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { SearchView } from './views/SearchView';
import { MovieDetailsView } from './views/MovieDetailsView';
import { MovieProvider } from './context/MovieContext';
import { ThemeProvider } from './theme/ThemeContext';
import styled from 'styled-components';
import { ThemeToggle } from './components';

const AppContainer = styled.div`
  min-height: 100vh;
  background-color: ${({ theme }) => theme.colors.background};
  color: ${({ theme }) => theme.colors.text.primary};
  transition: background-color 0.3s, color 0.3s;
`;

const Container = styled.div`
  max-width: 1200px;
  margin: 0 auto;
  padding: ${({ theme }) => theme.spacing.xl};
`;

const TopBar = styled.div`
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: ${({ theme }) => theme.spacing.md};
`;

const Title = styled.h1`
  font-size: 2rem;
  color: ${({ theme }) => theme.colors.text.primary};
  margin-bottom: ${({ theme }) => theme.spacing.md};
`;

function App() {
  return (
    <ThemeProvider>
      <MovieProvider>
        <AppContainer>
          <BrowserRouter>
            <Container>
            <TopBar>
              <Title>What to watch...</Title>
              <ThemeToggle />
            </TopBar>
              <Routes>
                <Route path="/search" element={<SearchView />} />
                <Route path="/movie/:id" element={<MovieDetailsView />} />
                <Route path="/" element={<Navigate to="/search" replace />} />
              </Routes>
            </Container>
          </BrowserRouter>
        </AppContainer>
      </MovieProvider>
    </ThemeProvider>
  );
}

export default App;