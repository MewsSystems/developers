import styled from "styled-components";

const StyledInput = styled.input`
  font-family: Exo;
  font-size: 24px;
  padding-inline: 6px;
  width: -webkit-fill-available;
  color: black;
  background-color: white;
`;

const SearchInputWrapper = styled.div`
  margin-bottom: 24px;
  display: flex;
`;

interface ISearchInput {
  setSearch: React.Dispatch<React.SetStateAction<string>>
}

export function SearchInput(props:ISearchInput) {
  return (
    <SearchInputWrapper>
      <StyledInput
        type="text"
        placeholder="Start typing the name of a movie..."
        onChange={(event) => props.setSearch(event.target.value)}
      />
    </SearchInputWrapper>
  );
}
