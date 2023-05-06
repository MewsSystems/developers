import { MoviesFoundType } from "./types";

const cachedData: { [key: string]: MoviesFoundType } = {};

export const getCachedData = (inputValue: string, pageNumber: number) => {
  const cacheKey = `${inputValue}-${pageNumber}`;
  return cachedData[cacheKey] || null;
};

export const cacheData = (inputValue: string, pageNumber: number, data: MoviesFoundType) => {
  const cacheKey = `${inputValue}-${pageNumber}`;
  cachedData[cacheKey] = data;
};
