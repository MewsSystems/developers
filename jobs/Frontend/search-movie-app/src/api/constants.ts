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

export { BASE_URL, MOCKED_LIST_MOVIES, MOCKED_AXIOS_ERROR };
