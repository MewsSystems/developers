import styled from "styled-components";

export const StyledMainContent = styled.main`
  max-width: 100%;
  padding: 1.2rem 2.4rem;
  display: flex;
  justify-content: flex-start;
  align-content: flex-start;
  flex-wrap: wrap;
  gap: 1.2rem;
  overflow: auto;
  flex: 1;

  @media screen and (max-width: 1024px) {
    padding: 1.2rem;
  }
`;
