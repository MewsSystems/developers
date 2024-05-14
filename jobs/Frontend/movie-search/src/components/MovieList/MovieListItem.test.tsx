import { render, screen } from '@testing-library/react';
import MovieListItem from './MovieListItem';
import {
	OVERVIEW_UNAVAILABLE_TEXT,
} from '../../constants';

describe('MovieListItem component', () => {
	const mockMovie = {
		id: 1,
		title: 'Test Movie',
		overview: 'Test Overview',
		vote_average: 7.5,
		poster_path: 'testPosterPath.jpg',
	};

	it('renders movie details correctly', () => {
		render(<MovieListItem movie={mockMovie} />);

		// Check if movie details are rendered correctly
		expect(screen.getByText('Test Movie')).toBeInTheDocument();
		expect(screen.getByText('Test Overview')).toBeInTheDocument();
	});

	it('renders overview if available', () => {
		render(<MovieListItem movie={mockMovie} />);

		// Check if the overview is rendered if available
		expect(screen.getByText('Test Overview')).toBeInTheDocument();
	});

	it('renders placeholder text if overview is unavailable', () => {
		const movieWithoutOverview = { ...mockMovie, overview: undefined };
		render(<MovieListItem movie={movieWithoutOverview} />);

		// Check if the placeholder text is rendered if overview is unavailable
		expect(screen.getByText(OVERVIEW_UNAVAILABLE_TEXT)).toBeInTheDocument();
	});
});
