import type { Movie, MovieDetails, MovieListResponse, MovieSearchResponse } from "../../types/movie"

export const mockMovieBase: Movie = {
  id: 574475,
  title: "Final Destination Bloodlines",
  overview:
    "Plagued by a violent recurring nightmare, college student Stefanie heads home to track down the one person who might be able to break the cycle and save her family from the grisly demise that inevitably awaits them all.",
  poster_path: "/6WxhEvFsauuACfv8HyoVX6mZKFj.jpg",
  backdrop_path: "/uIpJPDNFoeX0TVml9smPrs9KUVx.jpg",
  release_date: "2025-05-14",
  vote_average: 7.223,
  vote_count: 1114,
  genre_ids: [27, 9648],
}

export const mockMovieLiloStitch: Movie = {
  id: 552524,
  title: "Lilo & Stitch",
  overview:
    "The wildly funny and touching story of a lonely Hawaiian girl and the fugitive alien who helps to mend her broken family.",
  poster_path: "/c32TsWLES7kL1uy6fF03V67AIYX.jpg",
  backdrop_path: "/7Zx3wDG5bBtcfk8lcnCWDOLM4Y4.jpg",
  release_date: "2025-05-17",
  vote_average: 7.115,
  vote_count: 686,
  genre_ids: [10751, 878, 35, 12],
}

export const mockMovieDragon: Movie = {
  id: 1087192,
  title: "How to Train Your Dragon",
  overview:
    "On the rugged isle of Berk, where Vikings and dragons have been bitter enemies for generations, Hiccup stands apart, defying centuries of tradition when he befriends Toothless, a feared Night Fury dragon. Their unlikely bond reveals the true nature of dragons, challenging the very foundations of Viking society.",
  poster_path: "/q5pXRYTycaeW6dEgsCrd4mYPmxM.jpg",
  backdrop_path: "/7HqLLVjdjhXS0Qoz1SgZofhkIpE.jpg",
  release_date: "2025-06-06",
  vote_average: 8.05,
  vote_count: 362,
  genre_ids: [28, 10751, 14],
}

export const mockMovieStraw: Movie = {
  id: 1426776,
  title: "STRAW",
  overview:
    "What will be her last straw? A devastatingly bad day pushes a hardworking single mother to the breaking point — and into a shocking act of desperation.",
  poster_path: "/t3cmnXYtxJb9vVL1ThvT2CWSe1n.jpg",
  backdrop_path: "/wnnu8htEZBLtwrke9QYfLKx6zjp.jpg",
  release_date: "2025-06-05",
  vote_average: 8.04,
  vote_count: 581,
  genre_ids: [53, 18, 80],
}

export const mockMovies: Movie[] = [
  mockMovieBase,
  mockMovieLiloStitch,
  mockMovieDragon,
  mockMovieStraw,
]

export const mockMovieDetails: MovieDetails = {
  adult: false,
  backdrop_path: "/uIpJPDNFoeX0TVml9smPrs9KUVx.jpg",
  belongs_to_collection: {
    id: 8864,
    name: "Final Destination Collection",
    poster_path: "/nr4MXTXK2in9gxYhFMQGni66KRJ.jpg",
    backdrop_path: "/y3aWOInifbGKXM34KjtcMITrZRZ.jpg",
  },
  budget: 50000000,
  genres: [
    {
      id: 27,
      name: "Horror",
    },
    {
      id: 9648,
      name: "Mystery",
    },
  ],
  homepage: "https://www.finaldestinationmovie.com",
  id: 574475,
  imdb_id: "tt9619824",
  origin_country: ["US"],
  original_language: "en",
  original_title: "Final Destination Bloodlines",
  overview:
    "Plagued by a violent recurring nightmare, college student Stefanie heads home to track down the one person who might be able to break the cycle and save her family from the grisly demise that inevitably awaits them all.",
  popularity: 1124.222,
  poster_path: "/6WxhEvFsauuACfv8HyoVX6mZKFj.jpg",
  production_companies: [
    {
      id: 12,
      logo_path: "/2ycs64eqV5rqKYHyQK0GVoKGvfX.png",
      name: "New Line Cinema",
      origin_country: "US",
    },
    {
      id: 48788,
      logo_path: null,
      name: "Practical Pictures",
      origin_country: "US",
    },
    {
      id: 252366,
      logo_path: null,
      name: "Freshman Year",
      origin_country: "",
    },
    {
      id: 252367,
      logo_path: null,
      name: "Fireside Films",
      origin_country: "US",
    },
    {
      id: 216687,
      logo_path: "/kKVYqekveOvLK1IgqdJojLjQvtu.png",
      name: "Domain Entertainment",
      origin_country: "US",
    },
  ],
  production_countries: [
    {
      iso_3166_1: "US",
      name: "United States of America",
    },
  ],
  release_date: "2025-05-14",
  revenue: 277393717,
  runtime: 110,
  spoken_languages: [
    {
      english_name: "English",
      iso_639_1: "en",
      name: "English",
    },
  ],
  status: "Released",
  tagline: "Death runs in the family.",
  title: "Final Destination Bloodlines",
  video: false,
  vote_average: 7.221,
  vote_count: 1109,
}

export const mockPopularMoviesResponse: MovieListResponse = {
  page: 1,
  results: mockMovies,
  total_pages: 500,
  total_results: 10000,
}

export const mockSearchMoviesResponse: MovieSearchResponse = {
  page: 1,
  results: [mockMovieBase],
  total_pages: 1,
  total_results: 1,
}

export const mockEmptySearchResponse: MovieSearchResponse = {
  page: 1,
  results: [],
  total_pages: 0,
  total_results: 0,
}

export const mockMovieDetailsLiloStitch: MovieDetails = {
  adult: false,
  backdrop_path: "/7Zx3wDG5bBtcfk8lcnCWDOLM4Y4.jpg",
  belongs_to_collection: null,
  budget: 0,
  genres: [
    { id: 10751, name: "Family" },
    { id: 878, name: "Science Fiction" },
    { id: 35, name: "Comedy" },
    { id: 12, name: "Adventure" },
  ],
  homepage: "",
  id: 552524,
  imdb_id: "tt0758988",
  origin_country: ["US"],
  original_language: "en",
  original_title: "Lilo & Stitch",
  overview:
    "The wildly funny and touching story of a lonely Hawaiian girl and the fugitive alien who helps to mend her broken family.",
  popularity: 425.3561,
  poster_path: "/c32TsWLES7kL1uy6fF03V67AIYX.jpg",
  production_companies: [
    {
      id: 2,
      logo_path: "/wdrCwmRnLFJhEoH8GSfymY85KHT.png",
      name: "Walt Disney Pictures",
      origin_country: "US",
    },
  ],
  production_countries: [
    {
      iso_3166_1: "US",
      name: "United States of America",
    },
  ],
  release_date: "2025-05-17",
  revenue: 0,
  runtime: 85,
  spoken_languages: [
    {
      english_name: "English",
      iso_639_1: "en",
      name: "English",
    },
  ],
  status: "Released",
  tagline: "There's one in every family.",
  title: "Lilo & Stitch",
  video: false,
  vote_average: 7.115,
  vote_count: 686,
}
