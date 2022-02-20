import { Routes, Route, BrowserRouter } from "react-router-dom";
import { useState, useEffect } from "react";
import { useAppDispatch, useAppSelector } from "./hooks/redux";
import { selectQuery } from "./redux/querySlice";
import Detail from "./pages/Detail";
import Home from "./pages/Home";
import { selectSearchResult, setSearchResult } from "./redux/searchResultSlice";
import useSearch from "./hooks/useSearch";

function App() {

  const [currPage, setCurrPage] = useState(1)

  const query = useAppSelector(selectQuery)
  let data = useSearch(query, currPage)

  const dispatch = useAppDispatch()

  useEffect(() => {
    dispatch(setSearchResult(data))
  })


  return (
    <BrowserRouter>
      {/* <Header /> */}
      <Routes>
        <Route path="/" element={<Home
          currPage={currPage}
          setCurrPage={setCurrPage}
        />}
        />
        <Route path="detail/:movieId" element={<Detail />} />
      </Routes>
      {/* <Footer /> */}
    </BrowserRouter>

  );
}

export default App;
