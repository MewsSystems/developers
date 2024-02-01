import { screen } from '@testing-library/react';
import { render } from '../../../tests/utils';
import { Movie } from '../../types/MovieSearchTypes';
import MoviesList from './MoviesList';

const movies: Movie[] = [
    {
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
    },
    {
        adult: false,
        backdrop_path: '/gg4zZoTggZmpAQ32qIrP5dtnkEZ.jpg',
        genre_ids: [28, 80],
        id: 891699,
        original_language: 'en',
        original_title: 'Silent Night',
        overview:
            "A tormented father witnesses his young son die when caught in a gang's crossfire on Christmas Eve. While recovering from a wound that costs him his voice, he makes vengeance his life's mission and embarks on a punishing training regimen in order to avenge his son's death.",
        popularity: 1345.446,
        poster_path: '/tlcuhdNMKNGEVpGqBZrAaOOf1A6.jpg',
        release_date: '2023-11-30',
        title: 'Silent Night',
        video: false,
        vote_average: 5.9,
        vote_count: 242,
    },
];

describe('MovieList', () => {
    it('should render MovieList', () => {
        render(<MoviesList movies={movies} />);
        expect(
            screen.getByText(
                'The Hunger Games: The Ballad of Songbirds & Snakes'
            )
        ).toBeInTheDocument();
        expect(screen.getByText('Silent Night')).toBeInTheDocument();
    });
});
