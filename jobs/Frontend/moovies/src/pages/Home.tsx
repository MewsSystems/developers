import { useState } from "react";
import SearchField from "../components/SearchField";
import SearchResults from "../components/SearchResults";

const Home = () => {

    const [query, setQuery] = useState("")

    return (
        <>
            <SearchField query={query} setQuery={setQuery} />
            <SearchResults query={query} />
        </>
    )
}

export default Home
