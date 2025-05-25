import styled from "styled-components";

export const SearchInput = styled.input`
  flex: 1;
  padding: 1rem;
  font-size: 1.1rem;
  border: 2px solid #eee;
  border-radius: 8px;
  transition: border-color 0.2s ease;

  &:focus {
    outline: none;
    border-color: #007bff;
  }

  &::placeholder {
    color: #999;
  }
`;