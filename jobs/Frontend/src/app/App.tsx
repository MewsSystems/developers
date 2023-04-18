import { Routes, Route } from "react-router-dom";
import Layout from "../components/Layout";
import MovieDetail from "../features/movies/MovieDetail";
import MovieSearch from "../features/movies/MovieSearch";

function App() {
	return (
		<Routes>
			<Route path="/" element={<Layout />}>
				<Route index element={<MovieSearch />} />
				<Route path="/movie/:movieId" element={<MovieDetail />} />
			</Route>
		</Routes>
	);
}

export default App;
