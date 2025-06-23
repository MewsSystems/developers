import { StyledInput, SearchContainer } from "./SearchInput.styles";

type SearchInputProps = {
  placeholder: string;
  handleChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
  query: string | null;
};
function SearchInput({ placeholder, handleChange, query }: SearchInputProps) {
  return (
    <SearchContainer>
      <StyledInput
        type="search"
        placeholder={placeholder}
        value={query || ""}
        onChange={handleChange}
      ></StyledInput>
    </SearchContainer>
  );
}

export default SearchInput;
