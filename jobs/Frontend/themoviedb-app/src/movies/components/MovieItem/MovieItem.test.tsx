import { screen } from '@testing-library/react';
import { render } from '../../../tests/utils';
import { Movie } from '../../types/MovieSearchTypes';
import MovieItem from './MovieItem';

const movie: Movie = {
    adult: false,
    backdrop_path: '/5a4JdoFwll5DRtKMe7JLuGQ9yJm.jpg',
    genre_ids: [18, 878, 28],
    id: 695721,
    original_language: 'en',
    original_title: 'The Hunger Games: The Ballad of Songbirds & Snakes',
    overview:
        '64 years before he becomes the tyrannical president of Panem, Coriolanus Snow sees a chance for a change in fortunes when he mentors Lucy Gray Baird, the female tribute from District 12.',
    popularity: 2016.503,
    poster_path: '/mBaXZ95R2OxueZhvQbcEWy2DqyO.jpg',
    release_date: '2023-11-15',
    title: 'The Hunger Games: The Ballad of Songbirds & Snakes',
    video: false,
    vote_average: 7.2,
    vote_count: 1341,
};

describe('MovieItem', () => {
    it('should render MovieItem', () => {
        render(<MovieItem {...movie} />);
        expect(
            screen.getByText(
                'The Hunger Games: The Ballad of Songbirds & Snakes'
            )
        ).toBeInTheDocument();
    });

    it('should have View Details button ', () => {
        render(<MovieItem {...movie} />);
        const viewDetailsBtn = screen.getByRole('button', {
            name: 'View details',
        });
        expect(viewDetailsBtn).toBeInTheDocument();
    });
});
