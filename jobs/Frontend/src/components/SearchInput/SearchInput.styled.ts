import { styled } from "styled-components";

export const SearchWrapper = styled.div`
  display: flex;
  justify-content: center;
  align-items: center;
  margin-bottom: 2rem;
  position: relative;
`;

export const SearchBar = styled.input`
  width: 100%;
  height: 40px;
  padding: 0.5rem;
  font-size: 1.2rem;
  border: 1px solid #888;
  border-radius: 8px;
  background-color: #333;
  color: #aaa;
  font-style: italic;
  outline: none;

  &::placeholder {
    color: #888;
  }
`;

export const ClearButton = styled.button`
    background-color: transparent;
    border: none;
    color: #aaa;
    cursor: pointer;
    position: absolute;
    margin: auto;
    top: 0;
    bottom: 0;
    right: 10px;
  }

  &:hover {
    color: #fff;
  }

  span {
    font-size: 1.2rem;
  }
`;
