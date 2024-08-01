import { Form } from "./components/Form/Form";
import { Header } from "./components/Header/Header";
import styled from "styled-components";

const AppStyled = styled.div`
  text-align: center;
  min-height: 100vh;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: start;
`;

function App() {
  return (
    <AppStyled>
      <Header />
      <Form />
    </AppStyled>
  );
}

export default App;
