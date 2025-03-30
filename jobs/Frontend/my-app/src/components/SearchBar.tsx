import styled from 'styled-components';

const StyledSearchBar = styled.input`
  max-width: 100%;
  width: 90vw;
  padding: 0.25rem 0.5rem;
  border: 1px solid lightgrey;
  border-radius: var(--br-rounded);
  transition: var(--transition-prim);

  &[type='text']:hover,
  &[type='text']:focus {
    border-color: none;
    outline: none;
    box-shadow: 0 0 5px rgba(192, 38, 211, 0.5);
  }

  @media (min-width: 600px) {
    width: 50vw;
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
          aria-label="Search bar"
        />
      </label>
    </>
  );
};
