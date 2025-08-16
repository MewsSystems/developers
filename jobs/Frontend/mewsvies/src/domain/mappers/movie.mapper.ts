import {
    MovieSearchParams,
    MovieSearchResultDTO,
    MovieDTO,
    MovieInformationDTO,
    MovieInformationGenreDTO,
    MovieInformationProductionCompanyDTO,
    MovieInformationProductionCountryDTO,
    MovieInformationSpokenLanguageDTO,
} from '../../api/interfaces/movie.interface.ts'
import {
    MovieSearch,
    MovieSearchResult,
    Movie,
    MovieInformation,
    MovieInformationGenre,
    MovieInformationProductionCompany,
    MovieInformationProductionCountry,
    MovieInformationSpokenLanguage,
} from '../interfaces/movie.interface.ts'

export const mapSearchMovieToParams = (
    data: MovieSearch
): MovieSearchParams => ({
    query: data.query,
    page: `${data.page}`,
})

export const mapMovieFromDTO = (data: MovieDTO): Movie => ({
    adult: data.adult,
    backdrop_path: data.backdrop_path,
    genre_ids: data.genre_ids,
    id: data.id,
    original_language: data.original_language,
    original_title: data.original_title,
    overview: data.overview,
    popularity: data.popularity,
    poster_path: data.poster_path,
    release_date: data.release_date,
    title: data.title,
    video: data.video,
    vote_average: data.vote_average,
    vote_count: data.vote_count,
})

export const mapMovieSearchResultFromDTO = (
    data: MovieSearchResultDTO
): MovieSearchResult => ({
    page: data.page,
    total_pages: data.total_pages,
    total_results: data.total_results,
    results: data.results.map(mapMovieFromDTO),
})

export const mapMovieInformationGenreFromDTO = (
    data: MovieInformationGenreDTO
): MovieInformationGenre => ({
    id: data.id,
    name: data.name,
})

export const mapMovieInformationProductionCompanyFromDTO = (
    data: MovieInformationProductionCompanyDTO
): MovieInformationProductionCompany => ({
    id: data.id,
    logo_path: data.logo_path,
    name: data.name,
    origin_country: data.origin_country,
})

export const mapMovieInformationProductionCountryFromDTO = (
    data: MovieInformationProductionCountryDTO
): MovieInformationProductionCountry => ({
    iso_3166_1: data.iso_3166_1,
    name: data.name,
})

export const mapMovieInformationSpokenLanguageFromDTO = (
    data: MovieInformationSpokenLanguageDTO
): MovieInformationSpokenLanguage => ({
    english_name: data.english_name,
    iso_639_1: data.iso_639_1,
    name: data.name,
})

export const mapMovieInformationFromDTO = (
    data: MovieInformationDTO
): MovieInformation => ({
    adult: data.adult,
    backdrop_path: data.backdrop_path,
    belongs_to_collection: data.belongs_to_collection,
    budget: data.budget,
    genres: data.genres.map(mapMovieInformationGenreFromDTO),
    homepage: data.homepage,
    id: data.id,
    imdb_id: data.imdb_id,
    original_language: data.original_language,
    original_title: data.original_title,
    overview: data.overview,
    popularity: data.popularity,
    poster_path: data.poster_path,
    production_companies: data.production_companies.map(
        mapMovieInformationProductionCompanyFromDTO
    ),
    production_countries: data.production_countries.map(
        mapMovieInformationProductionCountryFromDTO
    ),
    release_date: data.release_date,
    revenue: data.revenue,
    runtime: data.runtime,
    spoken_languages: data.spoken_languages.map(
        mapMovieInformationSpokenLanguageFromDTO
    ),
    status: data.status,
    tagline: data.tagline,
    title: data.title,
    video: data.video,
    vote_average: data.vote_average,
    vote_count: data.vote_count,
})
