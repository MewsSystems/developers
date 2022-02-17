import SearchField from "../components/SearchField";
import SearchResults from "../components/SearchResults";

const Home = ({ results, setResults }: any) => {

    return (
        <>
            <SearchField setResults={setResults} />
            <SearchResults results={results} />
        </>
    )
}

export default Home
