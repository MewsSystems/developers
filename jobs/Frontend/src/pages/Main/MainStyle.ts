import styled from "styled-components";

export const MainContent = styled.main`
  flex: 1;
  padding: ${({ theme }) => theme.spacing(3)} ${({ theme }) => theme.spacing(4)};
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(180px, 1fr));
  gap: ${({ theme }) => theme.spacing(3)};
  align-content: start;
`;
