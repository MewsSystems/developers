'use client'

import { API_IMAGE_BASE_URL } from '@/const/endpoints'
import MovieDetailContext from '@/provider/MovieDetailProvider'
import Image from 'next/image'
import { useContext } from 'react'
import { MovieDetailEmptyState } from './MovieDetailEmptyState'

const MovieDetail = () => {
    const { movieQuery } = useContext(MovieDetailContext)
    const { data: movie } = movieQuery

    return movie ? (
        <div className="container mx-auto px-4 py-8">
            <div className="flex flex-col gap-8 md:flex-row">
                <div className="w-full md:w-1/3">
                    {movie?.poster_path ? (
                        <Image
                            src={`${API_IMAGE_BASE_URL}${movie?.poster_path}`}
                            alt={movie?.title || 'Movie poster'}
                            width={500}
                            height={750}
                            className="w-full rounded-lg shadow-lg"
                            priority
                        />
                    ) : (
                        <div className="flex aspect-[2/3] items-center justify-center rounded-lg bg-gray-200">
                            <span className="text-gray-400">
                                No image available
                            </span>
                        </div>
                    )}
                </div>
                <div className="flex w-full flex-col gap-y-6 md:w-2/3">
                    <h1 className="text-4xl font-bold">{movie?.title}</h1>
                    <h3 className="text-2xl font-bold">{movie?.tagline}</h3>
                    <p className="mb-4 text-gray-300">{movie?.overview}</p>

                    <div className="grid grid-cols-2 gap-4 text-gray-300">
                        <div>
                            <p>
                                <span className="font-semibold">
                                    Release Date:
                                </span>{' '}
                                {movie?.release_date}
                            </p>
                            <p>
                                <span className="font-semibold">Rating:</span>{' '}
                                {movie?.vote_average?.toFixed(1) ?? 'N/A'}/10
                            </p>
                        </div>
                        <div>
                            <p>
                                <span className="font-semibold">Language:</span>{' '}
                                {movie?.original_language?.toUpperCase() ??
                                    'N/A'}
                            </p>
                            <p>
                                <span className="font-semibold">
                                    Popularity:
                                </span>{' '}
                                {movie?.popularity?.toFixed(0) ?? 'N/A'}
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    ) : (
        <MovieDetailEmptyState />
    )
}

export default MovieDetail
