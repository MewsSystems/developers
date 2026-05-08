import { screen, fireEvent } from "@testing-library/react";
import { SearchMovieProvider } from "../contexts/SearchMovieContext";
import SearchMoviesView from "../pages/SearchMoviesView";
import { customRender } from "../utils/testUtils";

test("should update query in context when typing", () => {
	customRender(
		<SearchMovieProvider>
			<SearchMoviesView />
		</SearchMovieProvider>
	);
	const input: HTMLInputElement = screen.getByLabelText("Search for a movie");
	fireEvent.change(input, { target: { value: "action" } });
	expect(input.value).toBe("action");
});
