const BASE_URL = 'https://image.tmdb.org/t/p/';
const FILE_SIZE = 'w300';

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

export { BASE_URL, FILE_SIZE, MOCK_LIST_MOVIES_ONE_ITEM, MOCK_LIST_MOVIES_TWO_ITEMS };
