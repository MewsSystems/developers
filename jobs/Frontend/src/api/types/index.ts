interface HeaderConfig {
    headers: {
        'Content-Type': string
    }
}

export interface ApiConfig {
    baseUrl?: string
    apiKey?: string
    headerConfig: HeaderConfig
}

export interface CollectionResponse<T> {
    page: number
    results: T[]
    total_pages: number
    total_results: number
}
