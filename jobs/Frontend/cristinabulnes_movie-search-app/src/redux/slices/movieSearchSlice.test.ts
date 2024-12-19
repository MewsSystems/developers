import movieSearchReducer, { setQuery, loadMore } from "./movieSearchSlice";

const initialState = {
	query: "",
	page: 1,
	movies: [],
	isLoading: false,
	error: null,
	hasMore: false,
};

describe("movieSearchSlice", () => {
	it("should handle setQuery action", () => {
		const nextState = movieSearchReducer(initialState, setQuery("action"));
		expect(nextState.query).toBe("action");
		expect(nextState.page).toBe(1);
	});

	it("should handle loadMore action", () => {
		const nextState = movieSearchReducer(initialState, loadMore());
		expect(nextState.page).toBe(2);
	});
});
