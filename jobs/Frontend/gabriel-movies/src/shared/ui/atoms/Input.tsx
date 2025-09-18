import styled from "styled-components";

export const Input = styled.input`
  width: 100%;
  padding: 0.6rem 0.8rem;
  border: 2px solid ${({ theme }) => theme.colors.link};
  border-radius: 20px;
  &::placeholder { opacity: 0.7; }
  &:focus { outline: none; }
`;