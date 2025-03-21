// import { useState } from 'react';
import styled from 'styled-components';

const StyledSearchBar = styled.input`
  width: 50%;
  border: 1px solid lightgrey;
  border-radius: 10px;

  &[type='text']:hover,
  &[type='text']:focus {
    border-color: #007bff;
    outline: none;
    box-shadow: 0 0 5px rgba(0, 123, 255, 0.5);
  }
`;

export const SearchBar = () => {
  //   const [query, setQuery] = useState('');
  return (
    <>
      <label>
        <StyledSearchBar
          type="text"
          placeholder="Enter movie name or a keyword"
          //   onChange={(e) => setQuery(e.target.value)}
        />
      </label>
    </>
  );
};
