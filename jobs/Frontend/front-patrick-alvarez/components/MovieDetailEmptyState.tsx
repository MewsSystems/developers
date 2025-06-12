import Link from 'next/link'
import { memo } from 'react'

export const MovieDetailEmptyState = memo(() => {
    return (
        <div className="flex h-full w-full items-center justify-center">
            <div className="flex flex-col items-center gap-y-4">
                <h2 className="text-6xl font-bold">Oops!</h2>
                <h3 className="text-2xl font-bold">
                    This movie does not seem to exist...
                </h3>
                <Link href="/" className="text-blue-500">
                    <button className="rounded-md bg-blue-500 px-4 py-2 text-white">
                        Go back to the home page
                    </button>
                </Link>
            </div>
        </div>
    )
})

MovieDetailEmptyState.displayName = 'MovieDetailEmptyState'
