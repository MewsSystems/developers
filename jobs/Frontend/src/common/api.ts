import type { Movie, MovieSearchResponse } from "./movie";

const baseUrl = new URL(`https://api.themoviedb.org/3/`);
baseUrl.searchParams.set("api_key", process.env.REACT_APP_MOVIESDB_APIKEY!);
baseUrl.searchParams.set("language", "en");

async function request(url: URL) {
	const response = await fetch(url, {
		headers: {
			"Content-Type": "application/json",
		},
		method: "GET",
	});

	if (!response.ok) {
		const err = await response.text();
		throw new Error(err);
	}

	return await response.json();
}

function search(title: string, page: number): URL {
	const url = new URL(baseUrl);
	url.pathname += "search/movie";
	url.searchParams.set("include_adult", "false");
	url.searchParams.set("query", title);
	if (page > 1) {
		url.searchParams.set("page", String(page));
	}

	return url;
}

function movieDetails(id: string): URL {
	const url = new URL(baseUrl);
	url.pathname += `movie/${id}`;

	return url;
}

function movieCrew(id: string): URL {
	const url = new URL(baseUrl);
	url.pathname += `movie/${id}/credits`;

	return url;
}

export async function findMovies(title: string, page: number): Promise<MovieSearchResponse> {
	const data = await request(search(title, page));

	return {
		currentPage: data.page,
		totalPages: data.total_pages,
		results: data.results.map((r: any) => ({
			id: r.id,
			title: r.title,
			imageUrl: r.poster_path ? `https://image.tmdb.org/t/p/w200${r.poster_path}` : null,
		})),
	};
}

export async function findMovie(id: string): Promise<Movie> {
	const data = await Promise.all([request(movieDetails(id)), request(movieCrew(id))]).then((data) => {
		return Object.assign({}, ...data);
	});

	const result: Movie = {
		id: String(data.id),
		title: data.title,
		overview: data.overview,
		yearReleased: data.release_date.slice(0, 4),
		posterUrl: data.poster_path,
		cast: data.cast.map((c: any) => ({ name: c.name, character: c.character })).slice(0, 10),
		genres: data.genres.map((g: any) => g.name),
		directors: data.crew.filter((c: any) => c.job === "Director").map((c: any) => c.name),
	};

	return result;
}
