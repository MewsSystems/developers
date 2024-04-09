import { render, screen } from "@testing-library/react";
import { MemoryRouter, Routes, Route } from "react-router-dom";
import { QueryClient, QueryClientProvider } from "react-query";
import { MovieDetails } from ".././MovieDetails";

const queryClient = new QueryClient();

jest.mock("react-router-dom", () => ({
    ...jest.requireActual("react-router-dom"),
    useParams: () => ({ id: "1" }),
    useNavigate: () => jest.fn(),
}));

jest.mock("react-query", () => ({
    ...jest.requireActual("react-query"),
    useQueryClient: () => ({
        getQueryData: () => ({
            results: [
                {
                    id: 1,
                    title: "Movie Title",
                    overview: "Movie Overview",
                    release_date: "2022-01-01",
                    original_language: "en",
                    poster_path: "poster_path",
                    vote_average: 8.5,
                },
            ],
        }),
    }),
}));

describe("MovieDetails", () => {
    test("renders movie details correctly", () => {
        render(
            <QueryClientProvider client={queryClient}>
                <MemoryRouter initialEntries={["/movie/1"]}>
                    <Routes>
                        <Route path="/movie/:id" element={<MovieDetails />} />
                    </Routes>
                </MemoryRouter>
            </QueryClientProvider>,
        );

        expect(screen.getByText("Movie Title")).toBeInTheDocument();
        expect(screen.getByText("Movie Overview")).toBeInTheDocument();
        expect(screen.getByText("Release Date: 2022-01-01")).toBeInTheDocument();
        expect(screen.getByText("Original Language: en")).toBeInTheDocument();
        expect(screen.getByText("Vote Average: 8.5")).toBeInTheDocument();
        expect(screen.getByAltText("Movie Title")).toBeInTheDocument();
    });
});
