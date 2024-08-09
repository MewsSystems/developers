import React from "react";
import styled from "styled-components";
import { SearchInputProps } from "../types/SearchInterfaces";

const StyledSearchInput = styled.input`
  width: 90%;
  padding: 0.5rem;
  font-size: 1rem;
  padding: 20px;
  border: 1px solid #ccc;
  border-radius: 10px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.2);
  &:focus {
    outline: none;
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.4);
  }

  @media (min-width: 768px) {
    width: 50%;
  }

  @media (min-width: 1200px) {
    width: 30%;
  }
`;

const SearchInput: React.FC<SearchInputProps> = ({ value, onChange }) => {
  return (
    <div>
      <StyledSearchInput
        type="text"
        placeholder="Search for a movie..."
        value={value}
        onChange={onChange}
      />
    </div>
  );
};

export default SearchInput;
