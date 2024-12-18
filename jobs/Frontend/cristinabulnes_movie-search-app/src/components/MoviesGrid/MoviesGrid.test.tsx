import { useEffect } from "react";
import { MemoryRouter, Route, Routes } from "react-router-dom";
import { customRender, screen, fireEvent } from "../../utils/testUtils";
import MoviesGrid from "./MoviesGrid";
import { mockMovies } from "../../__mocks__/mockMovies";

const mockNavigate = jest.fn();
const mockLoadMore = jest.fn(() => console.log("mockLoadMore called"));

jest.mock("react-router-dom", () => ({
	...jest.requireActual("react-router-dom"),
	useNavigate: () => mockNavigate,
}));

jest.mock("../../hooks/useIntersectionObserver", () => ({
	useIntersectionObserver: jest.fn(
		(callback: () => void, ref: React.RefObject<HTMLElement>) => {
			// Simulate an intersection event by invoking the callback
			useEffect(() => {
				if (ref.current) {
					callback();
				}
			}, [ref, callback]);
		}
	),
}));

describe("MoviesGrid", () => {
	beforeEach(() => {
		jest.clearAllMocks();
	});

	test("renders a list of MovieCard components", () => {
		customRender(
			<MemoryRouter>
				<MoviesGrid
					movies={mockMovies}
					loadMore={mockLoadMore}
					hasMore={true}
				/>
			</MemoryRouter>
		);

		// Check that MovieCard components are rendered
		expect(
			screen.getByTestId(`movie-card-${mockMovies[0].id}`)
		).toBeInTheDocument();
		expect(
			screen.getByTestId(`movie-card-${mockMovies[1].id}`)
		).toBeInTheDocument();
	});

	test("triggers navigation when a MovieCard is clicked", () => {
		customRender(
			<MemoryRouter initialEntries={["/"]}>
				<MoviesGrid
					movies={mockMovies}
					loadMore={mockLoadMore}
					hasMore={true}
				/>
				<Routes>
					<Route path="/movie/:id" element={<div>Movie Details</div>} />
				</Routes>
			</MemoryRouter>
		);

		// Simulate a click on the first movie
		const movieCard1 = screen.getByTestId(`movie-card-${mockMovies[0].id}`);
		fireEvent.click(movieCard1);

		// Check that navigate was called with the correct URL
		expect(mockNavigate).toHaveBeenCalledWith(`/movie/${mockMovies[0].id}`);
	});

	test("calls loadMore when the last movie is visible", () => {
		customRender(
			<MemoryRouter>
				<MoviesGrid
					movies={mockMovies}
					loadMore={mockLoadMore}
					hasMore={true}
				/>
			</MemoryRouter>
		);

		// Verify loadMore is triggered
		expect(mockLoadMore).toHaveBeenCalled();
	});

	test("does not call loadMore if hasMore is false", () => {
		customRender(
			<MemoryRouter>
				<MoviesGrid
					movies={mockMovies}
					loadMore={mockLoadMore}
					hasMore={false}
				/>
			</MemoryRouter>
		);

		// Verify that loadMore is not called when hasMore is false
		expect(mockLoadMore).not.toHaveBeenCalled();
	});
});
