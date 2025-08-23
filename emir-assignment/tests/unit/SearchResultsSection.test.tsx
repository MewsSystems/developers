import { render, screen, fireEvent } from "@testing-library/react";
import SearchResultsSection from "../../src/features/search/SearchResultsSection";
import { describe, it, expect, vi } from "vitest";
import { MemoryRouter } from "react-router-dom";

// Dummy movie
const mockMovie = {
    id: 1,
    title: "Inception",
    overview: "A mind-bending thriller",
    poster_path: "/poster.jpg",
    backdrop_path: "/backdrop.jpg",
    release_date: "2010-07-16",
    vote_average: 8.8,
    vote_count: 10000,
};

describe("SearchResultsSection", () => {
    it("shows CardSkeleton while loading and no items", () => {
        render(
            <MemoryRouter>
                <SearchResultsSection
                    debounced="test"
                    items={[]}
                    page={1}
                    totalPages={null}
                    loading={true}
                    error={null}
                    isDebouncing={false}
                    canLoadMore={false}
                    onLoadMore={() => {}}
                    onRetry={() => {}}
                />
            </MemoryRouter>
        );

        expect(screen.getAllByTestId("skeleton-card")).toHaveLength(12);
    });

    it("shows EmptyState when no results found", () => {
        render(
            <MemoryRouter>
                <SearchResultsSection
                    debounced="Nonexistent Movie"
                    items={[]}
                    page={1}
                    totalPages={null}
                    loading={false}
                    error={null}
                    isDebouncing={false}
                    canLoadMore={false}
                    onLoadMore={() => {}}
                    onRetry={() => {}}
                />
            </MemoryRouter>
        );

        expect(screen.getByText(/No results/i)).toBeInTheDocument();
        expect(
            screen.getByText(
                /We couldn’t find anything for “Nonexistent Movie”/
            )
        ).toBeInTheDocument();
    });

    it("renders movie cards when items exist", () => {
        render(
            <MemoryRouter>
                <SearchResultsSection
                    debounced="Inception"
                    items={[mockMovie]}
                    page={1}
                    totalPages={1}
                    loading={false}
                    error={null}
                    isDebouncing={false}
                    canLoadMore={false}
                    onLoadMore={() => {}}
                    onRetry={() => {}}
                />
            </MemoryRouter>
        );

        expect(screen.getAllByText("Inception")).toHaveLength(2);
    });

    it("displays error state and calls retry on click", () => {
        const retryMock = vi.fn();

        render(
            <MemoryRouter>
                <SearchResultsSection
                    debounced="ErrorMovie"
                    items={[]}
                    page={1}
                    totalPages={null}
                    loading={false}
                    error={new Error("Something went wrong")}
                    isDebouncing={false}
                    canLoadMore={false}
                    onLoadMore={() => {}}
                    onRetry={retryMock}
                />
            </MemoryRouter>
        );

        expect(screen.getByText(/Something went wrong/i)).toBeInTheDocument();

        const button = screen.getByRole("button", { name: /Try again/i });
        fireEvent.click(button);
        expect(retryMock).toHaveBeenCalled();
    });

    it("calls onLoadMore when 'Load more' is clicked", () => {
        const loadMoreMock = vi.fn();

        render(
            <MemoryRouter>
                <SearchResultsSection
                    debounced="Inception"
                    items={[mockMovie]}
                    page={1}
                    totalPages={2}
                    loading={false}
                    error={null}
                    isDebouncing={false}
                    canLoadMore={true}
                    onLoadMore={loadMoreMock}
                    onRetry={() => {}}
                />
            </MemoryRouter>
        );

        const button = screen.getByRole("button", { name: /Load more/i });
        fireEvent.click(button);
        expect(loadMoreMock).toHaveBeenCalled();
    });
});
