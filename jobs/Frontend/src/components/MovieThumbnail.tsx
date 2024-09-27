import type { MovieThumbnailProps } from '../types'
import { Button, Divider, Stack, Typography } from '@mui/material'

import { useNavigate } from 'react-router-dom'
import { MoviePoster } from './MoviePoster'

export const MovieThumbnail = (props: MovieThumbnailProps) => {
    const { id, poster_path, title, release_date, ...elementProps } = props
    const navigate = useNavigate()

    return (
        <Stack
            className='h-full overflow-hidden rounded-md border-solid border-primary-main'
            {...elementProps}
        >
            <MoviePoster
                poster_path={poster_path}
                title={title}
            />
            <Divider />
            <Stack className='flex-1 justify-between gap-3 p-4'>
                <Stack>
                    {release_date && (
                        <Typography className='font-light'>
                            {release_date.substring(0, 4)}
                        </Typography>
                    )}
                    <Typography className='line-clamp-2 font-medium'>
                        {title}
                    </Typography>
                </Stack>
                <Button
                    onClick={() => navigate(`/movie/${id}`)}
                    variant='contained'
                >
                    see details
                </Button>
            </Stack>
        </Stack>
    )
}
