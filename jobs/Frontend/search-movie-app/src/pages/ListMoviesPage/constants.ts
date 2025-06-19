const BASE_URL = 'https://image.tmdb.org/t/p/';
const FILE_SIZE_LIST_MOVIES = 'w300';
const FILE_SIZE_DETAILS_MOVIE = 'w400';

const MOCK_LIST_MOVIES_ONE_ITEM = [
  {
    id: 1,
    title: 'Batman',
    voteAverage: 7.5,
    releaseDate: '2020-01-01',
    imageURL: '',
    isAdult: false,
    voteTotalCount: 150,
  },
];

const MOCK_LIST_MOVIES_TWO_ITEMS = [
  ...MOCK_LIST_MOVIES_ONE_ITEM,
  {
    id: 2,
    title: 'Superman',
    voteAverage: 7.8,
    releaseDate: '2023-12-12',
    imageURL: '',
    isAdult: false,
    voteTotalCount: 170,
  },
];

const RESULT_NOT_FOUND_TITLE = 'Result not found';
const RESULT_NOT_FOUND_SUBTITLE = `We couln't find the movie you're looking for`;
const SHOW_MORE_BUTTON_LABEL = 'Show more';

export {
  BASE_URL,
  MOCK_LIST_MOVIES_ONE_ITEM,
  MOCK_LIST_MOVIES_TWO_ITEMS,
  FILE_SIZE_LIST_MOVIES,
  FILE_SIZE_DETAILS_MOVIE,
  RESULT_NOT_FOUND_TITLE,
  RESULT_NOT_FOUND_SUBTITLE,
  SHOW_MORE_BUTTON_LABEL,
};
