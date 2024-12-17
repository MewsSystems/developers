import styled from "styled-components";

export const SearchBarContainer = styled.div`
  position: relative;
  width: 90%;
  max-width: 400px;
  margin: auto;

  @media (min-width: 768px) {
    max-width: 600px;
  }
`;

export const SearchInput = styled.input`
  width: 100%;
  padding: ${({ theme }) => theme.spacing(1.5)}
    ${({ theme }) => theme.spacing(2)};
  border-radius: 25px;
  border: none;
  outline: none;
  background: rgba(255, 255, 255, 0.15);
  color: ${({ theme }) => theme.colors.textPrimary};
  font-size: 1rem;

  &::placeholder {
    color: ${({ theme }) => theme.colors.textSecondary};
    font-size: 1rem;
  }

  &:focus {
    background: rgba(255, 255, 255, 0.25);
  }
`;
