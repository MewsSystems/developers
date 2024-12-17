import { SearchBarContainer, SearchInput } from "./SearchBarStyle";

const SearchBar: React.FC<
  React.DetailedHTMLProps<
    React.InputHTMLAttributes<HTMLInputElement>,
    HTMLInputElement
  >
> = (props) => {
  return (
    <SearchBarContainer>
      <SearchInput {...props} />
    </SearchBarContainer>
  );
};

export default SearchBar;
