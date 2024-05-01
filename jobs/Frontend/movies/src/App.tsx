import styled from 'styled-components';

function App() {
  return (
    <PageContainer>
      <h1>Something</h1>
    </PageContainer>
  );
}

const PageContainer = styled.main`
  max-width: 1280px;
  margin: 0 auto;
  padding: 2rem;
  text-align: center;
`;

export default App;
