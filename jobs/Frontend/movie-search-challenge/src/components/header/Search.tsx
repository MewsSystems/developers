import useSearch from "../../hooks/useSearch"
import { StyledSearch } from "./Search.styled"

const Search = () => {
  const [searchValue, setSearchValue] = useSearch()

  return (
    <form id="form">
      <StyledSearch
        name="search"
        onChange={event => setSearchValue(event.target.value)}
        placeholder="Search"
        type="text"
        value={searchValue}
      />
    </form>
  )
}
export default Search
