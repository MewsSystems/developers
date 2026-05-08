import { renderHook, waitFor } from "@testing-library/react";
import { afterEach, beforeEach, describe, expect, it, vi } from "vitest";

import { useTmdbSearch } from "../../../src/hooks/useTmdbSearch";
import {
    fetchJson,
    type TmdbMovie,
    type TmdbPage,
} from "../../../src/lib/tmdb";

// Mock the TMDB client
vi.mock("../../../src/lib/tmdb", async () => {
    const actual = await vi.importActual<
        typeof import("../../../src/lib/tmdb")
    >("../../../src/lib/tmdb");

    return {
        ...actual,
        fetchJson: vi.fn(),
    };
});

const mockedFetch = vi.mocked(fetchJson);

// helpers
function makeMovies(count: number, offset = 0): TmdbMovie[] {
    return Array.from({ length: count }, (_, i) => ({
        id: i + 1 + offset,
        title: `Movie ${i + 1 + offset}`,
        overview: "Overview",
        poster_path: null,
        backdrop_path: null,
        release_date: "2020-01-01",
        vote_average: 8,
        vote_count: 100,
    }));
}

function pageResponse(
    results: TmdbMovie[],
    page: number,
    total_pages = 5
): TmdbPage<TmdbMovie> {
    return {
        page,
        results,
        total_pages,
        total_results: results.length * total_pages,
    };
}

beforeEach(() => {
    mockedFetch.mockReset();
});

afterEach(() => {
    vi.clearAllMocks();
});

describe("useTmdbSearch", () => {
    it("returns idle/empty state when query is empty", async () => {
        const { result, rerender } = renderHook(
            ({ q, p }) => useTmdbSearch(q, p),
            { initialProps: { q: "", p: 1 } }
        );

        expect(result.current.items).toEqual([]);
        expect(result.current.totalPages).toBeNull();
        expect(result.current.loading).toBe(false);
        expect(result.current.error).toBeNull();
        expect(mockedFetch).not.toHaveBeenCalled();

        // if provide a query, it should fetch
        mockedFetch.mockResolvedValueOnce(pageResponse(makeMovies(30), 1, 10));
        rerender({ q: "inception", p: 1 });

        await waitFor(() => {
            expect(result.current.loading).toBe(false);
        });

        // slice to exactly 18
        expect(result.current.items).toHaveLength(18);
        expect(result.current.totalPages).toBe(10);
        expect(mockedFetch).toHaveBeenCalledWith(
            "/search/movie",
            { query: "inception", page: 1 },
            expect.any(Object)
        );
    });

    it("fetches first page and slices to 18 results", async () => {
        mockedFetch.mockResolvedValueOnce(pageResponse(makeMovies(50), 1, 5));

        const { result } = renderHook(() => useTmdbSearch("matrix", 1));

        // loading starts true then false when resolved
        expect(result.current.loading).toBe(true);

        await waitFor(() => {
            expect(result.current.loading).toBe(false);
        });

        expect(result.current.items).toHaveLength(18); // sliced
        expect(result.current.items[0].title).toBe("Movie 1");
        expect(result.current.items[17].title).toBe("Movie 18");
        expect(result.current.totalPages).toBe(5);
        expect(result.current.error).toBeNull();
    });

    it("appends when moving to the next page", async () => {
        // page 1
        mockedFetch.mockResolvedValueOnce(
            pageResponse(makeMovies(30, 0), 1, 3)
        );

        const { result, rerender } = renderHook(
            ({ q, p }) => useTmdbSearch(q, p),
            { initialProps: { q: "dark", p: 1 } }
        );

        await waitFor(() => expect(result.current.loading).toBe(false));
        expect(result.current.items).toHaveLength(18);

        // page 2
        mockedFetch.mockResolvedValueOnce(
            pageResponse(makeMovies(30, 100), 2, 3)
        );
        rerender({ q: "dark", p: 2 });

        await waitFor(() => expect(result.current.loading).toBe(false));
        // appended: 18 (page1) + 18 (page2) = 36
        expect(result.current.items).toHaveLength(36);
        expect(result.current.items[0].id).toBe(1);
        expect(result.current.items[18].id).toBe(101);
    });

    it("does not refetch if the same page was already loaded", async () => {
        // initial fetch for page 1
        mockedFetch.mockResolvedValueOnce(pageResponse(makeMovies(25), 1, 2));

        const { result, rerender } = renderHook(
            ({ q, p }) => useTmdbSearch(q, p),
            { initialProps: { q: "blade", p: 1 } }
        );

        await waitFor(() => expect(result.current.loading).toBe(false));
        expect(mockedFetch).toHaveBeenCalledTimes(1);

        // rerender with same query+page should not trigger another fetch
        rerender({ q: "blade", p: 1 });
        expect(mockedFetch).toHaveBeenCalledTimes(1);
    });

    it("resets items when query changes", async () => {
        mockedFetch
            .mockResolvedValueOnce(pageResponse(makeMovies(25), 1, 2)) // for "a"
            .mockResolvedValueOnce(pageResponse(makeMovies(25, 200), 1, 2)); // for "b"

        const { result, rerender } = renderHook(
            ({ q, p }) => useTmdbSearch(q, p),
            { initialProps: { q: "a", p: 1 } }
        );

        await waitFor(() => expect(result.current.loading).toBe(false));
        expect(result.current.items[0].id).toBe(1);

        // change query -> resets + fetches anew
        rerender({ q: "b", p: 1 });
        await waitFor(() => expect(result.current.loading).toBe(false));
        expect(result.current.items[0].id).toBe(201);
    });
});
