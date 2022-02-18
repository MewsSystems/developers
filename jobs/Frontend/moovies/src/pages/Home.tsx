import SearchField from "../components/SearchField";
import SearchResults from "../components/SearchResults";

const Home = (props: any) => {

    const { query, setQuery, searchResult, setSearchResult, isLoading, currPage, setCurrPage } = props

    return (
        <>
            <SearchField query={query} setQuery={setQuery} />
            <SearchResults
                query={query}
                searchResult={searchResult}
                setSearchResult={setSearchResult}
                isLoading={isLoading}
                currPage={currPage}
                setCurrPage={setCurrPage} />
        </>
    )
}

export default Home
