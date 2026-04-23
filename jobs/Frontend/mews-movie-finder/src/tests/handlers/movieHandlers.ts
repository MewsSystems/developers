import { http, HttpResponse } from "msw";
import { Movie, MovieDetail } from "../../types/movies";

export const movies: Movie[] = [
  {
    adult: false,
    backdrop_path: "/x2RS3uTcsJJ9IfjNPcgDmukoEcQ.jpg",
    genre_ids: [12, 14, 28],
    id: 120,
    original_language: "en",
    original_title: "The Lord of the Rings: The Fellowship of the Ring",
    overview:
      "Young hobbit Frodo Baggins, after inheriting a mysterious ring from his uncle Bilbo, must leave his home in order to keep it from falling into the hands of its evil creator. Along the way, a fellowship is formed to protect the ringbearer and make sure that the ring arrives at its final destination: Mt. Doom, the only place where it can be destroyed.",
    popularity: 117.078,
    poster_path: "/6oom5QYQ2yQTMJIbnvbkBL9cHo6.jpg",
    release_date: "2001-12-18",
    title: "The Lord of the Rings: The Fellowship of the Ring",
    video: false,
    vote_average: 8.405,
    vote_count: 23533,
  },
];

export const moviedetail: MovieDetail = {
  adult: false,
  backdrop_path: "/x2RS3uTcsJJ9IfjNPcgDmukoEcQ.jpg",
  belongs_to_collection: {
    id: 119,
    name: "The Lord of the Rings Collection",
    poster_path: "/oENY593nKRVL2PnxXsMtlh8izb4.jpg",
    backdrop_path: "/bccR2CGTWVVSZAG0yqmy3DIvhTX.jpg",
  },
  budget: 93000000,
  genres: [
    {
      id: 12,
      name: "Adventure",
    },
    {
      id: 14,
      name: "Fantasy",
    },
    {
      id: 28,
      name: "Action",
    },
  ],
  homepage: "http://www.lordoftherings.net/",
  id: 120,
  imdb_id: "tt0120737",
  original_language: "en",
  original_title: "The Lord of the Rings: The Fellowship of the Ring",
  overview:
    "Young hobbit Frodo Baggins, after inheriting a mysterious ring from his uncle Bilbo, must leave his home in order to keep it from falling into the hands of its evil creator. Along the way, a fellowship is formed to protect the ringbearer and make sure that the ring arrives at its final destination: Mt. Doom, the only place where it can be destroyed.",
  popularity: 117.078,
  poster_path: "/6oom5QYQ2yQTMJIbnvbkBL9cHo6.jpg",
  production_companies: [
    {
      id: 12,
      logo_path: "/mevhneWSqbjU22D1MXNd4H9x0r0.png",
      name: "New Line Cinema",
      origin_country: "US",
    },
    {
      id: 11,
      logo_path: "/6FAuASQHybRkZUk08p9PzSs9ezM.png",
      name: "WingNut Films",
      origin_country: "NZ",
    },
    {
      id: 5237,
      logo_path: null,
      name: "The Saul Zaentz Company",
      origin_country: "US",
    },
  ],
  production_countries: [
    {
      iso_3166_1: "NZ",
      name: "New Zealand",
    },
    {
      iso_3166_1: "US",
      name: "United States of America",
    },
  ],
  release_date: "2001-12-18",
  revenue: 871368364,
  runtime: 179,
  spoken_languages: [
    {
      english_name: "English",
      iso_639_1: "en",
      name: "English",
    },
  ],
  status: "Released",
  tagline: "One ring to rule them all",
  title: "The Lord of the Rings: The Fellowship of the Ring",
  video: false,
  vote_average: 8.405,
  vote_count: 23533,
};

const handleSearchValid = () =>
  HttpResponse.json({
    results: movies,
    page: 1,
    total_pages: 1,
    total_results: 2,
  });
const handleSearchEmpty = () =>
  HttpResponse.json({
    results: [],
    page: 1,
    total_pages: 0,
    total_results: 0,
  });

const handleSearch = (url: string) => {
  if (url.includes("lord")) {
    return handleSearchValid();
  }
  if (url.includes("error")) {
    throw new Error();
  }
  return handleSearchEmpty();
};

const handleDetailsValid = () => HttpResponse.json(moviedetail);

export const movieHandlers = [
  http.get("*3/search/movie", ({ request }) => handleSearch(request.url)),
  http.get("/movie/120", handleDetailsValid),
  http.get("/movie/42", () => {
    throw new Error();
  }),
];
