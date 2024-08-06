import React from "react";
import styled from "styled-components";
import { SearchInputProps } from "../types/SearchInterfaces";

const StyledSearchInput = styled.input`
  padding: 0.5rem;
  font-size: 1rem;
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
