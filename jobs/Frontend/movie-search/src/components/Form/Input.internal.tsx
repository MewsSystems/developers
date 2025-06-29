import styled from "styled-components";

export const InputWrapper = styled.div`
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: flex-start;
  width: 100%;
  max-width: 100%;
`;

export const StyledInput = styled.input`
  border: 1px solid #666;
  border-radius: 15rem;
  font-family: inherit;
  height: 2.4rem;
  width: 30%;
  padding: 0.8rem;

  @media screen and (max-width: 1024px) {
    width: 100%;
  }
`;
