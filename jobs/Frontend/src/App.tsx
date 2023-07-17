import { BrowserRouter, Navigate, Route, Routes } from "react-router-dom";

import { GlobalStyles } from "./global-styles";
import { Search } from "./pages/Search";
import { Details } from "./pages/Details";
import { styled } from "styled-components";

const Container = styled.div`
  width: 100%;
  height: 100%;

  background-color: lightgray;

  padding: 24px;

  overflow: auto;
`;

function App() {
  return (
    <Container>
      <GlobalStyles />

      <BrowserRouter>
        <Routes>
          <Route path="/" Component={Search} />
          <Route path="/movies/:movieId" Component={Details} />

          <Route path="*" element={<Navigate replace to="/" />} />
        </Routes>
      </BrowserRouter>
    </Container>
  );
}

export default App;
