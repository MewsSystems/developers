import { Movie } from "@/types";
import { MovieByIdAdapter } from "../MovieByIdAdapter";

describe('MovieByIdAdapter', () => {
  it('should adapt to Movie', () => {
    const actual: Movie = MovieByIdAdapter({
      id: 1,
      title: 'test title',
      homepage: 'homepage',
      genres: [{id: 1, name: 'romance'}],
      overview: 'overview',
      poster_path: '/poster',
      spoken_languages: [ {
        english_name: 'english',
        iso_639_1: 'en',
        name: 'english',
      }]
    });

    expect(actual).toEqual({
      id: 1,
      title: 'test title',
      overview: 'overview',
      posterImage: 'https://image.tmdb.org/t/p/w185/poster',
      homepage: 'homepage',
      genres: ['romance'],
      spokenLanguages: ['english'],
    })
  });
})