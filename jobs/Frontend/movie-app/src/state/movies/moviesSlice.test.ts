import { fetchPopularMovies } from './moviesSlice';
import { store } from "../store";
import { popularMoviesMockedReponse, popularMoviesMockedSecondPageReponse } from "./movies.mock";
import { PayloadAction } from "@reduxjs/toolkit";
import { MoviesReponseType } from "../../types";

describe('moviesSlice', () => {
    describe('fetchPopularMovies', () => {
        beforeEach(() => {
            process.env.REACT_APP_MOVIE_API_KEY = 'some-api-key';
        });

        it('should try fetch popular movies', async () => {
            global.fetch = jest.fn()

            store.dispatch(fetchPopularMovies({ page: 1 }))

            expect(fetch).toBeCalledWith('https://api.themoviedb.org/3/movie/popular?api_key=some-api-key&page=1')
        });

        it('should throw an error if fetched data does not match the MoviesResponse type', async () => {
            global.fetch = jest.fn().mockResolvedValue({
                json: jest.fn().mockResolvedValue({
                    ...popularMoviesMockedReponse,
                    results: 'invalid results type',
                }),
            });

            const response = await store.dispatch(fetchPopularMovies({ page: 1 }))

            // @ts-expect-error the response type doesn't have an error property - it's always MoviesReponseType
            expect(response?.error?.message).toEqual('Failed to validate movies data')
        })

        it('should return decoded data', async () => {
            global.fetch = jest.fn().mockResolvedValue({
                json: jest.fn().mockResolvedValue(popularMoviesMockedReponse),
            });

            const response = await store.dispatch(fetchPopularMovies({ page: 1 })) as PayloadAction<MoviesReponseType>

            expect(response.payload).toEqual(popularMoviesMockedReponse)
        })

        it('should store fetched movies', async () => {
            global.fetch = jest.fn().mockResolvedValue({
                json: jest.fn().mockResolvedValue(popularMoviesMockedReponse),
            });

            await store.dispatch(fetchPopularMovies({ page: 1 })) as PayloadAction<MoviesReponseType>

            const state = store.getState()

            expect(state.movies.popularMovies.length).toEqual(1)
            expect(state.movies.popularMovies[0].id).toEqual(popularMoviesMockedReponse.results[0].id)
            expect(state.movies.popularMoviesTotalPages).toEqual(popularMoviesMockedReponse.total_pages)
        })

        it('should store second fetched page', async () => {
            global.fetch = jest.fn().mockResolvedValueOnce({
                json: jest.fn().mockResolvedValueOnce(popularMoviesMockedReponse)
            }).mockResolvedValueOnce({
                json: jest.fn().mockResolvedValueOnce(popularMoviesMockedSecondPageReponse),
            });

            await store.dispatch(fetchPopularMovies({ page: 1 })) as PayloadAction<MoviesReponseType>
            await store.dispatch(fetchPopularMovies({ page: 2 })) as PayloadAction<MoviesReponseType>

            const state = store.getState()

            expect(state.movies.popularMoviesPage).toEqual(2)
            expect(state.movies.popularMovies[0].id).toEqual(popularMoviesMockedReponse.results[0].id)
            expect(state.movies.popularMovies[1].id).toEqual(popularMoviesMockedSecondPageReponse.results[0].id)
        })
    })
});