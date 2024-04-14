import type { MovieThumbnailProps } from '../types'
import { Button, Divider, Stack, Typography } from '@mui/material'
import fallbackImageSrc from '../assets/fallback-image.png'
import { useNavigate } from 'react-router-dom'

export const MovieThumbnail = (props: MovieThumbnailProps) => {
    const { id, poster_path, title, release_date, ...elementProps } = props
    const navigate = useNavigate()

    return (
        <Stack
            className='h-full overflow-hidden rounded-md border-solid border-primary-main'
            {...elementProps}
        >
            <img
                src={
                    poster_path
                        ? `https://media.themoviedb.org/t/p/w220_and_h330_face${poster_path}`
                        : fallbackImageSrc
                }
                alt={`${title} poster`}
                className='max-h-[18.75rem] min-h-[12.5rem] object-cover'
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
                    onClick={() => navigate(`movie/${id}`)}
                    variant='contained'
                >
                    see details
                </Button>
            </Stack>
        </Stack>
    )
}
