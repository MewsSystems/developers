import '@testing-library/jest-dom';
import React from "react";
import { fireEvent, render, screen } from "@testing-library/react";
import Movies from "../Movies";
import { Movie } from "@/types/Movie";
import { useRouter } from 'next/navigation';
import userEvent from '@testing-library/user-event';

jest.mock('next/navigation', () => ({
    useRouter: jest.fn()
}));

const movies: Movie[] = [
    {
        id: 1, original_title: "Movie 1", title: "Movie 1",
        overview: "overview",
        adult: false,
        backdrop_path: "",
        genre_ids: [1],
        popularity: 1,
        poster_path: "",
        release_date: ""
    },
    {
        id: 2, original_title: "Movie 2", title: "Movie 2",
        overview: "overview",
        adult: false,
        backdrop_path: "",
        genre_ids: [1],
        popularity: 1,
        poster_path: "",
        release_date: ""
    },
];

describe("Movies component", () => {
    test("renders movie cards correctly", () => {
        const push = jest.fn();

        (useRouter as jest.Mock).mockImplementation(() => ({
            push,
        }));
        render(<Movies movies={movies} />);
        expect(screen.getAllByTestId("card_movie").length).toBe(2);
    });

    test("calls push with correct URL when a movie is clicked", () => {
        const push = jest.fn();

        (useRouter as jest.Mock).mockImplementation(() => ({
            push,
        }));
        render(<Movies movies={movies} />);
        const movie = screen.getByText("Movie 2")
        fireEvent.click(movie);

        expect(push).toHaveBeenCalledWith(`/movies/${movies[1].id}`);
    });
});
