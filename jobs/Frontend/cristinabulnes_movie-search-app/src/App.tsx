import {
	BrowserRouter as Router,
	Routes,
	Route,
	useLocation,
} from "react-router-dom";
import "./App.css";
import SearchMoviesView from "./pages/SearchMoviesView";
import { ThemeProvider } from "styled-components";
import { theme } from "./theme";
import { SearchMovieProvider } from "./contexts/SearchMovieContext";
import MovieDetails from "./pages/MovieDetails";

const routes = {
	home: "/",
	movieDetails: "/movie/:movieId",
};

const AppRoutes = () => {
	const location = useLocation();
	const state = location.state as { backgroundLocation?: string };

	return (
		<>
			<Routes
				location={
					state?.backgroundLocation
						? { pathname: state.backgroundLocation }
						: location
				}
			>
				<Route path={routes.home} element={<SearchMoviesView />} />
			</Routes>

			{/* This route will be rendered on top of the page as a modal */}
			{state?.backgroundLocation && (
				<Routes>
					<Route path={routes.movieDetails} element={<MovieDetails />} />
				</Routes>
			)}
		</>
	);
};

const App = () => {
	return (
		<ThemeProvider theme={theme}>
			<SearchMovieProvider>
				<Router>
					<AppRoutes />
				</Router>
			</SearchMovieProvider>
		</ThemeProvider>
	);
};

export default App;
