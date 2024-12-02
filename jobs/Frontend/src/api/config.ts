import { ApiConfig } from './types'

export const getRestApiConfig = (): ApiConfig => {
    const headers = {
        'Content-Type': 'application/json',
    }

    return {
        baseUrl: import.meta.env.VITE_BASE_URL,
        apiKey: import.meta.env.VITE_API_KEY,
        headerConfig: {
            headers,
        },
    }
}
