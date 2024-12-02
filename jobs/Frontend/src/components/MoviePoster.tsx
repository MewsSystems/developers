import type { MoviePosterProps } from '../types'
import fallbackImageSrc from '../assets/fallback-image.png'
import { twMerge } from 'tailwind-merge'

export const MoviePoster = (props: MoviePosterProps) => {
    const { poster_path, title, className, ...elementProps } = props

    return (
        <img
            src={
                poster_path
                    ? `https://media.themoviedb.org/t/p/w220_and_h330_face${poster_path}`
                    : fallbackImageSrc
            }
            alt={`${title} poster`}
            className={twMerge(
                'max-h-[18.75rem] min-h-[12.5rem] object-cover',
                className,
            )}
            {...elementProps}
        />
    )
}
