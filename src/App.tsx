import { Header } from "./components/Header/Header";
import styled from "styled-components";

const AppStyled = styled.div`
  text-align: center;
`;

function App() {
  return (
    <AppStyled>
      <Header />
    </AppStyled>
  );
}

export default App;
