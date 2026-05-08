export const buildMovieDBUrl = (endpoint: string, query?: string, page = 1) => {
  const urlParams = new URLSearchParams({
    api_key: process.env.NEXT_PUBLIC_MOVIE_DB_API_KEY || "",
    include_adult: "false",
    language: "en-US",
    page: page.toString(),
    ...(query && { query }),
  });

  return `${
    process.env.NEXT_PUBLIC_MOVIE_DB_API_BASE_URL
  }/${endpoint}?${urlParams.toString()}`;
};
