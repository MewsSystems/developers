import styled from 'styled-components';

const StyledSearchBar = styled.input`
  max-width: 100%;
  width: 50vw;
  border: 1px solid lightgrey;
  border-radius: 10px;
  transition: all 0.3s ease-in-out;

  &[type='text']:hover,
  &[type='text']:focus {
    border-color: none;
    outline: none;
    box-shadow: 0 0 5px rgba(192, 38, 211, 0.5);
  }
`;

interface SearchBarProps {
  value: string;
  onSearchChange: (query: string) => void;
}

export const SearchBar = ({ value, onSearchChange }: SearchBarProps) => {
  const handleChange = (value: string) => {
    onSearchChange(value);
  };
  return (
    <>
      <label>
        <StyledSearchBar
          type="text"
          placeholder="Enter movie name or a keyword"
          value={value}
          onChange={(e) => handleChange(e.target.value)}
        />
      </label>
    </>
  );
};
