import { BrowserRouter, Navigate, Route, Routes } from "react-router-dom";
import styled from "styled-components";
import Header from "./components/Header";
import BrowseMovies from "./pages/BrowseMovies";
import MovieDetails from "./pages/MovieDetails";
import NoMatch from "./pages/NoMatch";
import { colors } from "./utils/theme";

const StyledBody = styled.body`
  font-family: "Open Sans";
  background: ${colors.appBackground};
  min-height: 100vh;
`;

const BoxedAppContainer = styled.div`
  max-width: 1200px;
  overflow: hidden;
  margin: auto;
`;

const StyledMain = styled.main``;

function App() {
  return (
    <>
      <BrowserRouter>
        <StyledBody>
          <BoxedAppContainer>
            <Header />
            <StyledMain>
              <Routes>
                <Route path="/:pageId" element={<BrowseMovies />} />
                <Route path="/" element={<Navigate to="/1" />} />
                <Route
                  path="/:currentPage/movie/:movieId"
                  element={<MovieDetails />}
                />
                <Route path="/movie/:movieId" element={<MovieDetails />} />
                <Route path="*" element={<NoMatch />} />
              </Routes>
            </StyledMain>
          </BoxedAppContainer>
        </StyledBody>
      </BrowserRouter>
    </>
  );
}

export default App;