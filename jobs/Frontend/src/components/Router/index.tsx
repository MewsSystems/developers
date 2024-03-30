import { BrowserRouter, Route, Routes } from "react-router-dom";
import { MovieDetailsView } from "../../views/MovieDetailsView";
import { MovieSearchView } from "../../views/MovieSearchView";
import { Layout } from "../Layout";

export const Router = () => {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Layout />}>
          <Route index element={<MovieSearchView />} />
          <Route path="/movie/:id" element={<MovieDetailsView />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
};
