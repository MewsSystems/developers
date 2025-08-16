import styled from "styled-components";

export const SearchContainer = styled.div`
  display: flex;
  justify-content: center;
  align-items: center;
  margin-bottom: 2rem;
`;

export const InputWrapper = styled.div`
  background-color: #333;
  display: flex;
  align-items: center;
  border-radius: 5px;
  padding: 10px;
`;

export const SearchIcon = styled.span`
  font-size: 1.5em;
  color: #666;
  margin-right: 10px;
`;

export const SearchInput = styled.input`
  background-color: transparent;
  color: #fff;
  border: none;
  outline: none;
  font-size: 1em;
  width: 200px;

  &::placeholder {
    color: #aaa;
  }
`;
