import { http, HttpResponse } from "msw";

export const MoviesMockedResponse = http.get(
  "https://api.themoviedb.org/3/search/movie",
  () => {
    return HttpResponse.json({
      page: 1,
      total_pages: 19,
      total_results: 369,
      results: [
        {
          adult: false,
          backdrop_path: null,
          belongs_to_collection: null,
          budget: 0,
          genres: [
            {
              id: 16,
              name: "Animation",
            },
          ],
          homepage: "https://fionavp.weebly.com/wick2023.html",
          id: 1390246,
          imdb_id: null,
          origin_country: ["FR"],
          original_language: "en",
          original_title: "Wick",
          overview:
            "On a dark, stormy night a treasure hunter falls victim to his greed - and some very bad luck.",
          popularity: 0.0143,
          poster_path: "/6zpc3rYTS44hJkUVvNQ9h8Lw50T.jpg",
          production_companies: [],
          production_countries: [
            {
              iso_3166_1: "FR",
              name: "France",
            },
            {
              iso_3166_1: "GB",
              name: "United Kingdom",
            },
          ],
          release_date: "2023-07-27",
          revenue: 0,
          runtime: 2,
          spoken_languages: [],
          status: "Released",
          tagline: "",
          title: "Wick",
          video: false,
          vote_average: 0,
          vote_count: 0,
        },
      ],
    });
  }
);

export const MovieMockedResponse = http.get(
  "https://api.themoviedb.org/3/movie/:movieId",
  () => {
    return HttpResponse.json({
      adult: false,
      backdrop_path: null,
      belongs_to_collection: null,
      budget: 0,
      genres: [
        {
          id: 16,
          name: "Animation",
        },
      ],
      homepage: "https://fionavp.weebly.com/wick2023.html",
      id: 1390246,
      imdb_id: null,
      origin_country: ["FR"],
      original_language: "en",
      original_title: "Wick",
      overview:
        "On a dark, stormy night a treasure hunter falls victim to his greed - and some very bad luck.",
      popularity: 0.0143,
      poster_path: "/6zpc3rYTS44hJkUVvNQ9h8Lw50T.jpg",
      production_companies: [],
      production_countries: [
        {
          iso_3166_1: "FR",
          name: "France",
        },
        {
          iso_3166_1: "GB",
          name: "United Kingdom",
        },
      ],
      release_date: "2023-07-27",
      revenue: 0,
      runtime: 2,
      spoken_languages: [],
      status: "Released",
      tagline: "",
      title: "Wick",
      video: false,
      vote_average: 0.0,
      vote_count: 0,
    });
  }
);

export const MovieImagesResponse = http.get(
  "https://image.tmdb.org/t/p/original/*",
  () => new HttpResponse(null, { status: 200 })
);
