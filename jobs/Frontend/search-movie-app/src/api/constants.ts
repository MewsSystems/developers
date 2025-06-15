const BASE_URL = 'https://api.themoviedb.org/3';

const MOCKED_LIST_MOVIES = {
  adult: false,
  backdrop_path: '/a2K8ak4pGCHR3O0hMualcokXXpa.jpg',
  genre_ids: [10749, 53],
  id: 210763,
  original_language: 'kn',
  original_title: 'A',
  overview:
    'A film director fades away from the industry after the failed love with the heroine of his movie. After being rehabilitated, he learns that some people conspired his fall and decides to take revenge.',
  popularity: 0.5005,
  poster_path: '/y3DH68oWOGMZ637tqYHfTGU4Bif.jpg',
  release_date: '1998-02-12',
  title: 'A',
  video: false,
  vote_average: 5.709,
  vote_count: 110,
};

const MOCKED_DETAILS_MOVIE = {
  adult: false,
  backdrop_path: '/hQ4pYsIbP22TMXOUdSfC2mjWrO0.jpg',
  belongs_to_collection: {
    id: 1382526,
    name: "Aki Kaurismäki's Proletariat Trilogy",
    poster_path: '/bUrReoZFLGti6ehkBW0xw8f12MT.jpg',
    backdrop_path: '/zAUItK1Nr473DIe8gWMsZ0DMR7L.jpg',
  },
  budget: 0,
  genres: [
    { id: 35, name: 'Comedy' },
    { id: 18, name: 'Drama' },
    { id: 10749, name: 'Romance' },
    { id: 80, name: 'Crime' },
  ],
  homepage: '',
  id: 2,
  imdb_id: 'tt0094675',
  origin_country: ['FI'],
  original_language: 'fi',
  original_title: 'Ariel',
  overview:
    'A Finnish man goes to the city to find a job after the mine where he worked is closed and his father commits suicide.',
  popularity: 1.8037,
  poster_path: '/ojDg0PGvs6R9xYFodRct2kdI6wC.jpg',
  production_companies: [
    {
      id: 2303,
      logo_path: null,
      name: 'Villealfa Filmproductions',
      origin_country: 'FI',
    },
  ],
  production_countries: [
    {
      iso_3166_1: 'FI',
      name: 'Finland',
    },
  ],
  release_date: '1988-10-21',
  revenue: 0,
  runtime: 73,
  spoken_languages: [
    {
      english_name: 'Finnish',
      iso_639_1: 'fi',
      name: 'suomi',
    },
  ],
  status: 'Released',
  tagline: '',
  title: 'Ariel',
  video: false,
  vote_average: 7.109,
  vote_count: 353,
};

const MOCKED_AXIOS_ERROR = {
  isAxiosError: true,
  response: {
    data: {
      status_code: 7,
      status_message: 'Invalid API key: You must be granted a valid key.',
      success: false,
    },
  },
};

export { BASE_URL, MOCKED_LIST_MOVIES, MOCKED_DETAILS_MOVIE, MOCKED_AXIOS_ERROR };
