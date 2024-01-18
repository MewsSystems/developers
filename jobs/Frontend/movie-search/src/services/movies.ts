import { AllMovies, MoviesDetailsFull } from "@/lib/types";
import { GenericService } from "./generics";
import axios, { AxiosResponse } from "axios";

export class Movies extends GenericService<AllMovies> {
  constructor(protected url: string) {
    super();
    this.url = url;
  }

  public override async getSearch(movieName: string, page: number) {
    const urlWithQuery = `${this.url}search/movie?query=${encodeURIComponent(
      movieName
    )}&page=${page}&api_key=03b8572954325680265531140190fd2a`;
    const response: AxiosResponse = await axios.get(urlWithQuery);
    const data = response.data;
    return data;
  }

  public override async getOne(id: number): Promise<MoviesDetailsFull> {
    const urlWithQuery = `${this.url}movie/${id}?api_key=03b8572954325680265531140190fd2a`;
    const response: AxiosResponse = await axios.get(urlWithQuery);
    const data = response.data;
    return data;
  }
}

const moviesSearch = new Movies("https://api.themoviedb.org/3/");

export default moviesSearch;
