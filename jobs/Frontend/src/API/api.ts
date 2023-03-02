import { SearchMovieResult, Movie } from "./interfaces";

export class Api {
  constructor(private apiKey: string, private basePath: string) {
    this.apiKey = apiKey;
    this.basePath = basePath;
  }

  async get<T>(path: string, params: {} = {}): Promise<T> {
    const queryParams = Object.entries(params)
      .map(([key, value]) => `${key}=${value}`)
      .join('&')
    
    const response = await fetch(`${this.basePath}${path}?api_key=${this.apiKey}&${queryParams}`, {
      method: 'GET'
    });
    if (response.status !== 200) {
      throw new Error("Resource not fetched");
    }
    return await response.json() as T;
  }

  async searchMovie(searchTerm: string, page: string) {
    return await this.get<SearchMovieResult>('/search/movie', { query: searchTerm, page: page });
  }

  async movieDetails(movieId: string) {
    return await this.get<Movie>(`/movie/${movieId}`);
  }

  getImageUrl(path: string) {
    return path != null ? `https://image.tmdb.org/t/p/w500${path}` : undefined;
  }
}
