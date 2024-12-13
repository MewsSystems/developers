import { Route, Routes } from "react-router";
import Main from "@/pages/Main/Main";
import MovieDetails from "@/pages/MovieDetails/MovieDetails";

const Routing = () => {
  return (
    <Routes>
      <Route index element={<Main />} />
      <Route path="/movies/:movieId" element={<MovieDetails />} />
    </Routes>
  );
};

export default Routing;
