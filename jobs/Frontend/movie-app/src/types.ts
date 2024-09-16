import * as t from "io-ts";


export const Movie = t.type({
    adult: t.boolean,
    backdrop_path: t.union([t.null, t.string]),
    genre_ids: t.array(t.number),
    id: t.number,
    original_language: t.string,
    original_title: t.string,
    overview: t.string,
    popularity: t.number,
    poster_path: t.union([t.null, t.string]),
    release_date: t.string,
    title: t.string,
    video: t.boolean,
    vote_average: t.number,
    vote_count: t.number,
});

export type MovieType = t.TypeOf<typeof Movie>;

export const MoviesResponse = t.type({
    page: t.number,
    results: t.array(Movie),
    total_pages: t.number,
    total_results: t.number,
});

export type MoviesReponseType = t.TypeOf<typeof MoviesResponse>;

const Genre = t.type({
    id: t.number,
    name: t.string,
});

const ProductionCompany = t.type({
    id: t.number,
    logo_path: t.union([t.null, t.string]),
    name: t.string,
    origin_country: t.string,
});

const SpokenLanguage = t.type({
    english_name: t.string,
    iso_639_1: t.string,
    name: t.string,
});

const BelongsToCollection = t.type({
    id: t.number,
    name: t.string,
    poster_path: t.union([t.null, t.string]),
    backdrop_path: t.union([t.null, t.string]),
});

export const MovieDetail = t.type({
    adult: t.boolean,
    backdrop_path: t.union([t.null, t.string]),
    belongs_to_collection: t.union([BelongsToCollection, t.null]),
    budget: t.number,
    genres: t.array(Genre),
    homepage: t.union([t.null, t.string]),
    id: t.number,
    imdb_id: t.union([t.null, t.string]),
    original_language: t.string,
    original_title: t.string,
    overview: t.string,
    popularity: t.number,
    poster_path: t.union([t.null, t.string]),
    production_companies: t.array(ProductionCompany),
    production_countries: t.array(
        t.type({
            iso_3166_1: t.string,
            name: t.string,
        })
    ),
    release_date: t.string,
    revenue: t.number,
    runtime: t.union([t.null, t.number]),
    spoken_languages: t.array(SpokenLanguage),
    status: t.string,
    tagline: t.union([t.null, t.string]),
    title: t.string,
    video: t.boolean,
    vote_average: t.number,
    vote_count: t.number,
});

export type MovieDetailType = t.TypeOf<typeof MovieDetail>;