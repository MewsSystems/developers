import { render, screen } from '@testing-library/react';
import MovieDetail from './MovieDetail';
import { DATE_INPUT_FORMAT, DATE_OUTPUT_FORMAT, SEARCH_VIEW_ROUTE_PATH } from '../../constants';
import formatDate from '../../utilities/DateFormatter';

describe('MovieDetail component', () => {
	const mockMovie = {
		id: 1,
		title: 'Test Movie',
		tagline: 'Test Tagline',
		release_date: '2022-05-01',
		runtime: 120,
		genres: [
			{ id: 1, name: 'Action' },
			{ id: 2, name: 'Adventure' },
		],
		overview: 'Test Overview',
		vote_average: 7.5,
		vote_count: 1000,
		backdrop_path: 'testBackdropPath.jpg',
	};

	it('renders movie details correctly', () => {
		render(<MovieDetail movie={mockMovie} />);

		// Check if all movie details are rendered correctly
		expect(screen.getByText('Test Movie')).toBeInTheDocument();
		expect(screen.getByText('Test Tagline')).toBeInTheDocument();
		expect(screen.getByText('Release date: Sunday, 1st May 2022')).toBeInTheDocument();
		expect(screen.getByText('Runtime: 120m')).toBeInTheDocument();
		expect(screen.getByText('Action')).toBeInTheDocument();
		expect(screen.getByText('Adventure')).toBeInTheDocument();
		expect(screen.getByText('Test Overview')).toBeInTheDocument();
		expect(screen.getByText('1000 votes')).toBeInTheDocument();
	});

	it('renders new search link correctly', () => {
		render(<MovieDetail movie={mockMovie} />);

		// Check if new search link is rendered correctly
		const newSearchLink = screen.getByText('New search');
		expect(newSearchLink).toBeInTheDocument();
		expect(newSearchLink).toHaveAttribute('href', SEARCH_VIEW_ROUTE_PATH);
	});

	it('formats release date correctly', () => {
		// Format the release date using the same formatter used in the component
		const formattedDate = formatDate(mockMovie.release_date, DATE_INPUT_FORMAT, DATE_OUTPUT_FORMAT);

		render(<MovieDetail movie={mockMovie} />);

		// Check if the release date is formatted correctly
		expect(screen.getByText(`Release date: ${formattedDate}`)).toBeInTheDocument();
	});
});
