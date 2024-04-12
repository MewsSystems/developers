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
