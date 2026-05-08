import '@testing-library/jest-dom';
import React from 'react';
import { render, screen } from '@testing-library/react';
import Details from '.';

// Mocked MovieDetails data
const movieDetails = {
    title: 'Test Movie',
    original_title: 'Test Movie',
    release_date: '2022-01-01',
    genres: [{ id: 1, name: 'Action' }, { id: 2, name: 'Adventure' }],
    runtime: 120,
    vote_average: 7.5,
    tagline: 'Test Tagline',
    overview: 'Test Overview',
    backdrop_path: '/test-backdrop.jpg',
    poster_path: '/test-poster.jpg',
    adult: false,
    popularity: 1,
    revenue: 1,
    homepage: "",
    id: 1,
    spoken_languages: [], status: "", video: false, vote_count: 1
};

describe('Details component', () => {
    it('renders movie details correctly', () => {
        render(<Details movie={movieDetails} />);

        // Assert that movie details are rendered correctly
        expect(screen.getByText('Test Movie')).toBeInTheDocument();
        expect(screen.getByText('(2022)')).toBeInTheDocument();
        expect(screen.getByText(/2hr 0m/)).toBeInTheDocument(); // Assuming 120 minutes is 2 hours
        expect(screen.getByText('7')).toBeInTheDocument();
        expect(screen.getByText('User score')).toBeInTheDocument();
        expect(screen.getByText('Test Tagline')).toBeInTheDocument();
        expect(screen.getByText('Overview')).toBeInTheDocument();
        expect(screen.getByText('Test Overview')).toBeInTheDocument();
    });

});
