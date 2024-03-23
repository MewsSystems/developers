import React from 'react';
import { render, screen } from '@testing-library/react';
import MovieTile from '@/components/MovieTile';
import '@testing-library/jest-dom';
import userEvent from '@testing-library/user-event';

jest.mock('next/router', () => ({
    useRouter: jest.fn(),
}));

const mockMovie = {
    overview: 'This is a test overview for the movie',
    title: 'Test Movie',
    id: '123456',
    poster_path: 'test_poster_path.jpg',
};

describe('MovieTile component', () => {
    it('renders movie details correctly', () => {
        const useRouterMock = {
            push: jest.fn(),
        };
        require('next/router').useRouter.mockReturnValue(useRouterMock);

        render(<MovieTile movie={mockMovie} />);

        const titleElement = screen.getByText(mockMovie.title);
        const overviewElement = screen.getByText(/This is a test overview/);
        const linkElement = screen.getByText('See more');

        expect(titleElement).toBeInTheDocument();
        expect(overviewElement).toBeInTheDocument();
        expect(linkElement).toBeInTheDocument();
    });

    it('renders poster image with correct alt text', () => {
        render(<MovieTile movie={mockMovie} />);

        const imgElement = screen.getByAltText(mockMovie.title);
        expect(imgElement).toBeInTheDocument();
        expect(imgElement).toHaveAttribute('src', `https://image.tmdb.org/t/p/original/${mockMovie.poster_path}`);
    });

    it('navigates to correct movie detail page on link click', async () => {
        const useRouterMock = {
            push: jest.fn(),
        };

        require('next/router').useRouter.mockReturnValue(useRouterMock);

        render(<MovieTile movie={mockMovie} />);

        const linkElement = screen.getByText('See more');
        expect(linkElement).toBeInTheDocument();

        await userEvent.click(linkElement);

        expect(useRouterMock.push).toHaveBeenCalledTimes(1);
        expect(useRouterMock.push).toHaveBeenCalledWith(`/${mockMovie.id}`);
    });
});
