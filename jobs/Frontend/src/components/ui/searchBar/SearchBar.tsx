import styled from 'styled-components'

interface SearchBarProps extends React.InputHTMLAttributes<HTMLInputElement> {
  searchQuery: string
  setSearchQuery: React.Dispatch<React.SetStateAction<string>>
}

function SearchBar({
  placeholder,
  autoFocus,
  searchQuery,
  setSearchQuery,
}: SearchBarProps) {
  return (
    <StyledSearchBar
      className="search-bar"
      type="search"
      value={searchQuery}
      onChange={(e) => setSearchQuery(e.target.value)}
      placeholder={placeholder}
      autoFocus={autoFocus}
    />
  )
}

const StyledSearchBar = styled.input`
  border: 1px solid var(--primary-brand-color-100);
  border-radius: 1rem;
  padding: 0.5rem 1rem;
  background-color: var(--primary-brand-color-100);
  color: white;
  font-weight: 600;
  font-size: 1rem;

  &:focus-visible {
    outline-style: solid;
    outline-color: #de0b30;
    outline-width: 0.1rem;
  }

  &::placeholder {
    color: white;
    opacity: 0.5;
  }

  &::before {
    content: '🔍';
  }
`

export default SearchBar
