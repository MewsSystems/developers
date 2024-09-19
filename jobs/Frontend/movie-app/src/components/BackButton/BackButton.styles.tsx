import styled from "styled-components";

export const BackButtonContainer = styled.div`
  width: 100%;
  max-width: 40rem;
  padding-bottom: 1rem;
`;
export const StyledBackButton = styled.button`
  padding: 0.5rem 2rem;
  background: none;
  border: #808080 1px solid;
  cursor: pointer;
  border-radius: 1.5rem;
  &:hover {
    transform: scale(1.1);
  }
`;
