export const Loading = () => {
    return (
        <div
            className="flex min-h-screen items-center justify-center"
            role="status"
            aria-live="polite"
        >
            <div
                className="h-16 w-16 animate-spin rounded-full border-b-4 border-white"
                aria-label="Loading"
            />
            <span className="sr-only">Loading...</span>
        </div>
    )
}
