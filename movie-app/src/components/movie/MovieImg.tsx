import { HTMLAttributes } from 'react'

import { Box } from '@mui/material'

import { movieDbPosterPath } from '@/api/config.ts'

type MovieImgProps = {
    posterPath?: string | null
    isDetail?: boolean
} & HTMLAttributes<HTMLImageElement>

const MovieImg = (props: MovieImgProps) => {
    const { isDetail = false, posterPath, ...imgProps } = props

    return (
        <Box
            aria-hidden
            data-testid="movie-img"
            sx={{
                flexShrink: 0,
                height: isDetail ? 600 : 200,
                width: isDetail ? 400 : 300,
                maxWidth: 1,
                borderRadius: 5,
                backgroundRepeat: 'no-repeat',
                backgroundSize: 'cover',
                backgroundImage: posterPath
                    ? `url('${movieDbPosterPath}w220_and_h330_face${posterPath}')`
                    : 'linear-gradient(to right bottom, #02011D, #474860)',
            }}
            {...imgProps}
        />
    )
}

export default MovieImg
