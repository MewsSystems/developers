import SearchView from './routes/SearchView';
import MovieDetailView from './routes/MovieDetailView';
import { Routes, Route } from 'react-router';
import { MOVIE_DETAIL_VIEW_ROUTE_PATH, SEARCH_VIEW_ROUTE_PATH } from './constants';
import LogoLink from './components/Logo/LogoLink';
import './App.css';

function App() {
	return (
		<div className='page-container'>
			<LogoLink />

			<Routes>
				<Route path={SEARCH_VIEW_ROUTE_PATH} element={<SearchView />} />
				<Route path={MOVIE_DETAIL_VIEW_ROUTE_PATH} element={<MovieDetailView />} />
			</Routes>
		</div>
	);
}

export default App;
