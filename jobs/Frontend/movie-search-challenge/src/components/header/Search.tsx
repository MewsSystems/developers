import useSearch from "../../hooks/useSearch"
import { StyledSearch } from "./Search.styled"

const Search = () => {
  const [searchValue, setSearchValue] = useSearch()

  return (
    <form id="form">
      <StyledSearch
        type="text"
        value={searchValue}
        placeholder="Search"
        onChange={event => setSearchValue(event.target.value)}
      />
    </form>
  )
}
export default Search
