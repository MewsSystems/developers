import {
	BrowserRouter as Router,
	Routes,
	Route,
	useLocation,
} from "react-router-dom";
import { Provider } from "react-redux";
import { ThemeProvider } from "styled-components";
import store from "./redux/store";
import SearchMoviesView from "./pages/SearchMoviesView";
import MovieDetails from "./pages/MovieDetails";
import ErrorBoundary from "./components/ErrorBoundary";
import { theme } from "./theme";
import "./App.css";

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
				<Route path={routes.movieDetails} element={<MovieDetails />} />
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
			<Provider store={store}>
				<Router>
					<ErrorBoundary>
						<AppRoutes />
					</ErrorBoundary>
				</Router>
			</Provider>
		</ThemeProvider>
	);
};

export default App;
