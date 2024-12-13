class BaseClient {
  protected apiKey: string
  protected apiUrl: string
  protected defaultRequestOptions: RequestInit

  constructor(
    apiUrl: string,
    apiKey: string,
    defaultRequestOptions: RequestInit
  ) {
    this.apiKey = apiKey
    this.apiUrl = apiUrl
    this.defaultRequestOptions = defaultRequestOptions
  }
}

export default BaseClient
