import { RestClient, RestClientGetParams, RestClientPostParams } from "./types/rest-api";
import { API_CONFIG } from "./api-config";


class FetchRestClient implements RestClient {
  private readonly defaultOptions: RequestInit;

  constructor(private readonly host: string) {
    this.defaultOptions = {
      headers: API_CONFIG.headers
    };
  }

  private buildUrl(path?: string): string {
    return path ? `${this.host}${path}` : this.host;
  }

  private mergeOptions(options?: RequestInit): RequestInit {
    return {
      ...this.defaultOptions,
      ...options,
      headers: {
        ...this.defaultOptions.headers,
        ...options?.headers
      }
    };
  }

  private async handleResponse<T>(response: Response): Promise<T> {
    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }
    return await response.json();
  }

  async get<ResType>({ url, options }: RestClientGetParams): Promise<ResType> {
    const response = await fetch(
      this.buildUrl(url),
      this.mergeOptions({ ...options, method: 'GET' })
    );
    return this.handleResponse<ResType>(response);
  }

  async post<ResType>({ url, body, options }: RestClientPostParams): Promise<ResType> {
    const response = await fetch(
      this.buildUrl(url),
      this.mergeOptions({
        ...options,
        method: 'POST',
        body
      })
    );
    return this.handleResponse<ResType>(response);
  }
}

export const createApiClient = () => {
  return new FetchRestClient(API_CONFIG.baseUrl);
};

export { FetchRestClient };