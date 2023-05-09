import { Provider } from "react-redux";
import store from "./redux/store";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import { styled } from "styled-components";
import ListView from "@views/ListView";
import MovieDetail from "@components/MovieDetail/MovieDetail";

const AppWrapper = styled.div`
  max-width: 960px;
  margin: 0 auto;
  padding: 2rem 20px;
  margin-top: 2rem;
  background-color: #181818;
  color: #fff;
  min-height: 100vh;
`;

function App() {
  return (
    <Provider store={store}>
      <Router>
        <AppWrapper>
          <Routes>
            <Route path="/" element={<ListView />} />
            <Route path="/movie/:id" element={<MovieDetail />} />
          </Routes>
        </AppWrapper>
      </Router>
    </Provider>
  );
}

export default App;
