export interface RestClientGetParams {
  url?: string;
  options?: RequestInit;
}

export type RestClientPostParams = RestClientGetParams & {
  body?: BodyInit;
};

export interface RestClient {
  get: <ResType>({ url, options }: RestClientGetParams) => Promise<ResType>;
  post: <ResType>({ url, body, options }: RestClientPostParams) => Promise<ResType>;
}