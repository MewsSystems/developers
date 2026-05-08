import {
	mapFetchMovieDetailsResponse,
	mapFetchMoviesResponse,
} from "./responseMappers";

describe("mapFetchMoviesResponse", () => {
	it("should correctly map a valid API response to the Movie type", () => {
		const rawMovie = {
			id: "1",
			title: "Test Movie",
			poster_path: "/test.jpg",
			release_date: "2024-01-01",
			vote_average: 8.5,
			overview: "Test overview",
		};

		const expectedMovie = {
			id: "1",
			title: "Test Movie",
			posterPath: "/test.jpg",
			releaseDate: "2024-01-01",
			voteAverage: 8.5,
			overview: "Test overview",
		};

		expect(mapFetchMoviesResponse(rawMovie)).toEqual(expectedMovie);
	});

	it("should handle missing optional fields gracefully", () => {
		const rawMovie = {
			id: "2",
			title: "Another Movie",
			poster_path: null,
			release_date: "2024-01-01",
			vote_average: 7.5,
			overview: "Another overview",
		};

		const expectedMovie = {
			id: "2",
			title: "Another Movie",
			posterPath: null,
			releaseDate: "2024-01-01",
			voteAverage: 7.5,
			overview: "Another overview",
		};

		expect(mapFetchMoviesResponse(rawMovie)).toEqual(expectedMovie);
	});
});

describe("mapFetchMovieDetailsResponse", () => {
	it("should correctly map a valid API response to the Movie type", () => {
		const rawMovie = {
			id: "1",
			title: "Detailed Movie",
			poster_path: "/detailed.jpg",
			release_date: "2024-01-01",
			vote_average: 9.0,
			overview: "Detailed overview",
			genres: [
				{ id: 1, name: "Action" },
				{ id: 2, name: "Drama" },
			],
		};

		const expectedMovie = {
			id: "1",
			title: "Detailed Movie",
			posterPath: "/detailed.jpg",
			releaseDate: "2024-01-01",
			voteAverage: 9.0,
			genres: [
				{ id: 1, name: "Action" },
				{ id: 2, name: "Drama" },
			],
			overview: "Detailed overview",
		};

		expect(mapFetchMovieDetailsResponse(rawMovie)).toEqual(expectedMovie);
	});

	it("should handle missing genres gracefully", () => {
		const rawMovie = {
			id: "2",
			title: "No Genres Movie",
			poster_path: "/no-genres.jpg",
			release_date: "2024-01-01",
			vote_average: 5.0,
			overview: "No genres overview",
			genres: [],
		};

		const expectedMovie = {
			id: "2",
			title: "No Genres Movie",
			posterPath: "/no-genres.jpg",
			releaseDate: "2024-01-01",
			voteAverage: 5.0,
			genres: [],
			overview: "No genres overview",
		};

		expect(mapFetchMovieDetailsResponse(rawMovie)).toEqual(expectedMovie);
	});
});
