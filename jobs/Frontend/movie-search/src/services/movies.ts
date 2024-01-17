import { AllMovies } from "@/lib/types";
import { GenericService } from "./generics";
import axios, { AxiosResponse } from "axios";

export class Movies extends GenericService<AllMovies> {
  constructor(protected url: string) {
    super();
    this.url = url;
  }

  public override async getSearch(
    movieName: string,
    page: number
  ): Promise<AllMovies[]> {
    const urlWithQuery = `${this.url}&query=${encodeURIComponent(
      movieName
    )}&page=${page}`;
    const response: AxiosResponse = await axios.get(urlWithQuery);
    const data = response.data.results;
    return data;
  }
}

const allMoviesSearch = new Movies(
  "https://api.themoviedb.org/3/search/movie?api_key=03b8572954325680265531140190fd2a"
);

export default allMoviesSearch;
