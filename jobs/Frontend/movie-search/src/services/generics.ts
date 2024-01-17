import axios, { AxiosResponse } from "axios";

export abstract class GenericService<T> {
  protected abstract url: string;

  public async getSearch(searchParam: string, page: number): Promise<T[]> {
    const urlWithQuery = `${this.url}?query=${encodeURIComponent(
      searchParam
    )}&page=${page}`;
    const response: AxiosResponse = await axios.get(urlWithQuery);
    const data = response.data;
    return data;
  }
}
