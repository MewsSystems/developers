export interface RestClientGetParams {
  url: string;
  options?: RequestInit;
}

export interface RestClient {
  get: <ResType>({ url, options }: RestClientGetParams) => Promise<ResType>;
}