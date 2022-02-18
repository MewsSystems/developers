
import { Routes, Route, BrowserRouter } from "react-router-dom";
import { useState, useEffect } from "react";
import Header from "./components/Header";
import Footer from "./components/Footer";
import Detail from "./pages/Detail";
import Home from "./pages/Home";
import { SearchResult } from "./components/SearchResults";
import useSearch from "./hooks/useSearch";

function App() {

  const [query, setQuery] = useState("")
  const [searchResult, setSearchResult] = useState<SearchResult>({ data: [], error: false, loading: false })
  const [currPage, setCurrPage] = useState(1)

  let data = useSearch(query, currPage)

  useEffect(() => {
    setSearchResult(data)
  })


  return (
    <BrowserRouter>
      <Header />
      <Routes>
        <Route path="/" element={<Home
          query={query}
          setQuery={setQuery}
          searchResult={searchResult}
          setSearchResult={setSearchResult}
          currPage={currPage}
          setCurrPage={setCurrPage}
          isLoading={searchResult.loading} />}
        />
        <Route path="detail/:movieId" element={<Detail />} />
      </Routes>
      <Footer />
    </BrowserRouter>

  );
}

export default App;
