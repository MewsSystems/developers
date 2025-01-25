import { memo } from 'react'

const SKELETON_COUNT = 9

const SkeletonItem = memo(() => (
    <div className="animate-pulse">
        <div className="relative flex h-48 w-full flex-col rounded-lg bg-gray-700">
            <div className="absolute bottom-4 left-4">
                <div className="rounded-lg bg-gray-600 p-2">
                    <div className="h-4 w-32 rounded bg-gray-500"></div>
                    <div className="mt-1 h-3 w-16 rounded bg-gray-500"></div>
                </div>
            </div>
        </div>
    </div>
))

// Precompute the array since it never changes
const skeletonArray = [...Array(SKELETON_COUNT)].map((_, index) => index)

export const MovieListSkeletons = () => {
    return (
        <section className="grid w-full grid-cols-1 gap-6 md:grid-cols-2 lg:grid-cols-3">
            {skeletonArray.map((index) => (
                <SkeletonItem key={index} />
            ))}
        </section>
    )
}

SkeletonItem.displayName = 'SkeletonItem'
