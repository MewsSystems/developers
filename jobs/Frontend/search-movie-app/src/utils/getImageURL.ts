import { BASE_URL } from '../pages/ListMoviesPage/constants';

export const getImageURL = (filePath: string, fileSize: string) => {
  return `${BASE_URL}${fileSize}${filePath}`;
};
