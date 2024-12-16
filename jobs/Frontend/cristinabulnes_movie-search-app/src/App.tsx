import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import "./App.css";
import SearchMoviesView from "./pages/SearchMoviesView";
import MovieDetailsView from "./pages/MovieDetailsView";
import { ThemeProvider } from "styled-components";
import { theme } from "./theme";

const routes = {
	home: "/",
	movieDetails: "/movie/:movieId",
};

const App = () => {
	return (
		<ThemeProvider theme={theme}>
			<Router>
				<Routes>
					<Route path={routes.home} element={<SearchMoviesView />} />
					<Route path={routes.movieDetails} element={<MovieDetailsView />} />
				</Routes>
			</Router>
		</ThemeProvider>
	);
};

export default App;
