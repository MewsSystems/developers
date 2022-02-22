import { Routes, Route, BrowserRouter } from "react-router-dom";
import { useEffect } from "react";
import { useAppDispatch, useAppSelector } from "./hooks/redux";
import { selectQuery } from "./redux/querySlice";
import Detail from "./pages/Detail";
import Home from "./pages/Home";
import { setSearchResult } from "./redux/searchResultSlice";
import useSearch from "./hooks/useSearch";
import { selectCurrentPage } from "./redux/currentPageSlice";

function App() {

  // get state from redux store
  const query = useAppSelector(selectQuery)
  const currPage = useAppSelector(selectCurrentPage)

  // fetch data
  let data = useSearch(query, currPage)

  const dispatch = useAppDispatch()

  useEffect(() => {
    dispatch(setSearchResult(data))
  })

  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="detail/:movieId" element={<Detail />} />
        <Route path="*" element={<Home />} />
      </Routes>
    </BrowserRouter>

  );
}

export default App;
