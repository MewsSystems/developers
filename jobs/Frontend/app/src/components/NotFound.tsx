import styled from 'styled-components';

const CenteredFallback = styled.div`
  flex: 1;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.5rem;
`;

const NotFound = () => (
  <CenteredFallback>
    <div>404 - Page Not Found</div>
  </CenteredFallback>
);

export default NotFound;
