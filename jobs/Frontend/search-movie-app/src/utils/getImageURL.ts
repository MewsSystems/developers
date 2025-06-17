import { FILE_SIZE, BASE_URL } from '../pages/ListMoviesPage/constants';

const getImageURL = (filePath: string) => {
  return `${BASE_URL}${FILE_SIZE}${filePath}`;
};

export { getImageURL };
