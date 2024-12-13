// this function extracts the year from a string that represents the release date of a movie
// date is in format yyyy-mm-dd
export const extractYearFromReleaseDate = (date?: string): string => date?.split("-")[0] ?? "";
