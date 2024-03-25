type ImageSize = 200 | 500

export const getMoviePosterPath = (width: ImageSize, posterPath: string) => `https://image.tmdb.org/t/p/w${width}` + posterPath
