import { render, screen } from "@testing-library/react";
import { MemoryRouter } from "react-router-dom";
import MovieCard from "../../src/components/MovieCard";
import { describe, it, expect } from "vitest";

// Mock stars so we don't test it here
vi.mock("../../src/components/Stars", () => ({
    default: () => <div data-testid="stars" />,
}));

// Mock image helpers
vi.mock("../../src/lib/tmdb", () => {
    return {
        posterUrl: (path: string | null) =>
            path ? `https://image.tmdb.org/t/p/w342${path}` : null,
        backdropUrl: (path: string | null) =>
            path ? `https://image.tmdb.org/t/p/w780${path}` : null,
    };
});

const defaultProps = {
    id: 123,
    title: "Inception",
    release_date: "2010-07-16",
    vote_average: 8.8,
    poster_path: "/poster.jpg",
    backdrop_path: "/backdrop.jpg",
    overview: "A mind-bending thriller.",
};

describe("MovieCard", () => {
    it("renders title, year, and rating", () => {
        render(<MovieCard {...defaultProps} />, { wrapper: MemoryRouter });

        const titles = screen.getAllByText("Inception");
        expect(titles).toHaveLength(2);
        expect(screen.getAllByText("2010")).toHaveLength(2); // in overlay and below card
        expect(screen.getAllByTestId("stars")).toHaveLength(2); // in overlay and below
        expect(screen.getByText("4.4/5")).toBeInTheDocument();
    });

    it("renders poster image", () => {
        render(<MovieCard {...defaultProps} />, { wrapper: MemoryRouter });

        const img = screen.getByAltText("Inception") as HTMLImageElement;
        expect(img).toBeInTheDocument();
        expect(img.src).toContain("/poster.jpg");
    });

    it("uses fallback for missing poster", () => {
        render(<MovieCard {...defaultProps} poster_path={null} />, {
            wrapper: MemoryRouter,
        });
        expect(screen.queryByAltText("Inception")).not.toBeInTheDocument();
        expect(screen.getByTestId("no-poster")).toBeInTheDocument();
    });

    it("shows overview in hover overlay", () => {
        render(<MovieCard {...defaultProps} />, { wrapper: MemoryRouter });
        expect(screen.getByText(/mind-bending/i)).toBeInTheDocument();
    });

    it("links to the correct movie route", () => {
        render(<MovieCard {...defaultProps} />, { wrapper: MemoryRouter });

        const link = screen.getByRole("link", { name: /inception/i });
        expect(link.getAttribute("href")).toBe("/movie/123");
    });

    it("handles missing release_date gracefully", () => {
        render(<MovieCard {...defaultProps} release_date={undefined} />, {
            wrapper: MemoryRouter,
        });

        expect(screen.getAllByText("-")).toHaveLength(2); // default fallback year
    });
});
