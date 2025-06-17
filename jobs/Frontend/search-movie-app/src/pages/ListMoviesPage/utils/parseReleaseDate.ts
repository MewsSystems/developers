const parseReleaseDate = (releaseDate: string): string => {
  return new Date(releaseDate).getFullYear().toString();
};

export { parseReleaseDate };
