import { screen, render } from '@testing-library/react'
import {MovieDetailsView} from "@/views/MovieDetailsView";
import React from 'react';
import {MemoryRouter, Route, Routes} from "react-router-dom";
import {Movie} from "@/types";

vi.mock('@/services', () => ({
    movieService: {
        getMovieDetails: vi.fn().mockResolvedValue({
            id: 123,
            overview: 'Overview',
            poster_path: 'Image',
            release_date: '1978-11-15',
            title: 'Title',
        }) as Partial<Movie>
    }
}));

describe('Movie Details View', () => {
    it('should render the movie details', async () => {
        const testId = '123';

        render(
            <MemoryRouter initialEntries={[`/movie/${testId}`]}>
                <Routes>
                    <Route path="/movie/:id" element={<MovieDetailsView />} />
                </Routes>
            </MemoryRouter>
        )

        const overview = await screen.findByText(/Overview/)
        const title = await screen.findByText(/Title/)
        const release_date = await screen.findByText(/1978-11-15/)

        expect(overview).toBeInTheDocument()
        expect(title).toBeInTheDocument()
        expect(release_date).toBeInTheDocument()
    })
})
