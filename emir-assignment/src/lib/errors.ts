export interface HttpError extends Error {
    status?: number;
    data?: unknown;
    url?: string;
}
