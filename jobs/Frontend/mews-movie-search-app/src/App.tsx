import React from "react";
import styled from "styled-components";
import { Movies } from "./Movies";
import { Route } from "wouter";
import { MoviesDetailView } from "./MoviesDetailView";

function App() {
  const [search, setSearch] = React.useState("");

  return (
    <MainLayout>
      {/*  Movies View */}
      <Route
        path="/"
        component={() => (
          <>
            <HeaderNav>
              <Title>Mews Movie Search App</Title>
              <SearchBarContainer>
                <Input
                  aria-label="Search a movie"
                  type="text"
                  onChange={(e) => setSearch(e.currentTarget.value)}
                  placeholder="Search a movie"
                />
              </SearchBarContainer>
            </HeaderNav>
            <Movies searchTerm={search} />
          </>
        )}
      />

      {/*  Details Movie View */}
      <Route path="/movies/:id" component={MoviesDetailView} />
    </MainLayout>
  );
}

export default App;

const Input = styled.input`
  padding: 0.5rem;
  margin: 1rem;
  border: none;
  border-radius: 5px;
  width: 200px;
  font-size: 1rem;
`;

const Title = styled.h3`
  color: white;
  padding: 0.5rem 1rem;
`;
const HeaderNav = styled.nav`
  background: #1a1a1a;
  display: flex;
  justify-content: flex-start;
  align-items: center;
`;
const SearchBarContainer = styled.div`
  background: white;
`;

const MainLayout = styled.main`
  width: 100vw;
  height: 100vh;
  display: flex;
  background-color: #77b0aa;
  flex-direction: column;
  font-family: "Poppins", sans-serif;
  font-weight: 800;
`;
