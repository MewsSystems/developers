import {screen, render} from '@testing-library/react'
import React from 'react';
import { MoviesListView} from "@/views";
import {SearchMoviesAPIResponse} from "@/types";
import {userEvent} from "@testing-library/user-event";
import {MemoryRouter, Route, Routes} from "react-router-dom";
import {MovieSearchProvider} from "@/context";

const renderWithContext = ( Component: React.ReactElement ) => {
    render(
        <MemoryRouter initialEntries={['/']}>
            <Routes>
                <Route path="/" element={
                    <MovieSearchProvider>
                        {Component}
                    </MovieSearchProvider>
                } />
            </Routes>
        </MemoryRouter>

    )
}

vi.mock('@/services', () => ({
    movieService: {
        searchMovies: vi.fn().mockResolvedValue({
            page: 1,
            total_pages: 2,
            results: [
                {
                    id: 123,
                    overview: 'Overview',
                    poster_path: 'Image',
                    release_date: '1978-11-15',
                    title: 'Title',
                },
                {
                    id: 1234,
                    overview: 'Overview 2',
                    poster_path: 'Image 2',
                    release_date: '1978-11-15',
                    title: 'Title 2',
                }
            ]
        } as SearchMoviesAPIResponse)
    }
}));

describe('Movies List View', () => {
    it('should render the search bar', () => {
        render(<MoviesListView />)

        const searchBar = screen.getByPlaceholderText(/Search/i)

        expect(searchBar).toBeInTheDocument()
    })

    it('should allow the user to search for a movie', async () => {
        renderWithContext(<MoviesListView />)

        const searchBar = screen.getByPlaceholderText(/Search/i)
        await userEvent.click(searchBar)
        await userEvent.type(searchBar, 'Inception');
        const pagination = await screen.findByLabelText('pagination navigation');
        const movie = await screen.findByText('Title');
        const movie_2 = await screen.findByText('Title 2');

        expect(searchBar).toBeInTheDocument()
        expect(pagination).toBeInTheDocument()
        expect(movie).toBeInTheDocument()
        expect(movie_2).toBeInTheDocument()
    })
})
