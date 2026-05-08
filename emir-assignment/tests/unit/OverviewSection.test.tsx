import { describe, it, expect } from "vitest";
import { render, screen } from "@testing-library/react";
import OverviewSection from "../../src/features/movie/OverviewSection";

describe("OverviewSection", () => {
    const baseProps = {
        posterUrl: "https://example.com/poster.jpg",
        overview: "A thrilling adventure through time.",
        details: {
            genres: "Action, Sci-Fi",
            languages: "English, French",
            status: "Released",
            runtime: "2h 28m",
            releaseDate: "2010-07-16",
            budget: 160000000,
            revenue: 825532764,
            companies: ["Warner Bros.", "Legendary Pictures"],
        },
    };

    it("renders poster image, overview, and all detail rows", () => {
        render(<OverviewSection {...baseProps} />);

        expect(screen.getByRole("img")).toHaveAttribute(
            "src",
            baseProps.posterUrl
        );
        expect(screen.getByText("Storyline")).toBeInTheDocument();
        expect(screen.getByText(baseProps.overview)).toBeInTheDocument();
        expect(screen.getByText("Genres")).toBeInTheDocument();
        expect(screen.getByText("Languages")).toBeInTheDocument();
        expect(screen.getByText("Status")).toBeInTheDocument();
        expect(screen.getByText("Runtime")).toBeInTheDocument();
        expect(screen.getByText("Release date")).toBeInTheDocument();
        expect(screen.getByText("Budget")).toBeInTheDocument();
        expect(screen.getByText("Revenue")).toBeInTheDocument();
        expect(screen.getByText("Production")).toBeInTheDocument();
    });

    it("renders fallback for missing overview", () => {
        render(<OverviewSection {...baseProps} overview="" />);
        expect(screen.getByText(/No overview available/i)).toBeInTheDocument();
    });

    it("does not render budget, revenue, or companies if not provided", () => {
        const partialDetails = {
            genres: "Drama",
            languages: "English",
            status: "Post Production",
            runtime: "1h 30m",
            releaseDate: "2025-01-01",
        };

        render(<OverviewSection {...baseProps} details={partialDetails} />);

        expect(screen.queryByText("Budget")).not.toBeInTheDocument();
        expect(screen.queryByText("Revenue")).not.toBeInTheDocument();
        expect(screen.queryByText("Production")).not.toBeInTheDocument();
    });

    it("handles empty posterUrl without crashing", () => {
        render(<OverviewSection {...baseProps} posterUrl="" />);
        expect(screen.queryByRole("img")).not.toBeInTheDocument();
    });
});
