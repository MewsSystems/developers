import styled from "styled-components";

function App() {
  return <Layout>This is the beginning</Layout>;
}

export default App;

const Layout = styled.div`
  width: 100vw;
  height: 100vh;
  display: grid;
  place-content: center;
  background-color: #77b0aa;
  font-family: "Poppins", sans-serif;
  font-weight: 800;
  font-size: 2rem;
`;
