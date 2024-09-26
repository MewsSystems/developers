import { expect, test } from 'vitest';
import { MovieMapper } from '../src/mapping/movieMapper';
import { TmdbDto } from '../src/interfaces/tmdbDto';
import { MovieModel } from '../src/interfaces/movieModel';
import { PosterImageSize } from '../src/enums/images/posterImageSize';
import { BackdropImageSize } from '../src/enums/images/backdropImageSize';

test('moviePreviewFromDto maps data', () => {
    const moviePreviewDto: TmdbDto.MoviePreview = {
        adult: false,
        backdrop_path: '/my-path',
        id: 123,
        original_language: 'en',
        original_title: 'A movie',
        overview: 'Lorem Ipsum',
        popularity: '12',
        poster_path: '/my-poster-path',
        title: 'The title',
        video: false,
        vote_average: 6.6,
        vote_count: 5,
        genre_ids: [1,2,3]
    };

    const moviePreviewModel: Partial<MovieModel.MoviePreview> = {
        adult: false,
        id: 123,
        originalLanguage: 'en',
        originalTitle: 'A movie',
        overview: 'Lorem Ipsum',
        popularity: '12',
        title: 'The title',
        video: false,
        voteAverage: 6.6,
        voteCount: 5
    };

    const mappingResult = MovieMapper.moviePreviewFromDto(moviePreviewDto);

    expect(mappingResult).contain(moviePreviewModel);
    expect(mappingResult.getPosterUrl(PosterImageSize.Width342)).toEqual('https://image.tmdb.org/t/p/w342/my-poster-path');
    expect(mappingResult.getBackdropUrl(BackdropImageSize.Width1280)).toEqual('https://image.tmdb.org/t/p/w1280/my-path');
});

// TODO: test additional mappings