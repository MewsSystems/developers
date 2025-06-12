import { memo } from 'react'

export const MovieSearchEmptyState = memo(() => {
    return (
        <div className="flex h-full w-full items-center justify-center">
            <div className="flex flex-col items-center gap-y-4">
                <h2 className="text-6xl font-bold">Oops!</h2>
                <h3 className="text-2xl font-bold">No movies found</h3>
                <p className="text-sm text-gray-500">
                    Try searching for a different movie
                </p>
            </div>
        </div>
    )
})

MovieSearchEmptyState.displayName = 'MovieSearchEmptyState'
