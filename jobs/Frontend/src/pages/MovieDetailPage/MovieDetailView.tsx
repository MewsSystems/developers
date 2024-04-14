import type { MovieDetailPageProps } from './types'
import type { FC } from 'react'
import {
    Box,
    Button,
    Chip,
    Grid,
    Rating,
    Skeleton,
    Stack,
    Tooltip,
    Typography,
} from '@mui/material'
import {
    AccessTime,
    ArrowBack,
    CalendarToday,
    Home,
    Language,
} from '@mui/icons-material'
import { MoviePoster } from '../../components'

export const MovieDetailView: FC<MovieDetailPageProps> = (props) => {
    const {
        detailData,
        isLoading,
        isError,
        navigateBack,
        navigateHome,
        isPreviousPageAvailable,
    } = props

    const stopedLoadingWithData = !isLoading && detailData

    return (
        <Box className='relative pb-7'>
            <Box className=' absolute inset-x-0 top-0 -z-[1] bg-gray-100 sm:h-[18rem]' />
            <Box
                component='main'
                className='container pt-8'
            >
                <Stack className='mb-6 flex-row items-center gap-2'>
                    <Button
                        variant='text'
                        color='inherit'
                        className='text-primary-contrastText'
                        startIcon={<ArrowBack />}
                        onClick={navigateBack}
                        disabled={!isPreviousPageAvailable}
                    >
                        back
                    </Button>
                    <Typography>|</Typography>
                    <Button
                        variant='text'
                        color='inherit'
                        className='text-primary-contrastText'
                        startIcon={<Home />}
                        onClick={navigateHome}
                    >
                        home
                    </Button>
                </Stack>
                <Grid
                    container
                    spacing={3}
                >
                    <Grid
                        item
                        xs={6}
                        sm={4}
                        className='relative'
                    >
                        {stopedLoadingWithData ? (
                            <MoviePoster
                                poster_path={detailData?.poster_path}
                                title={detailData?.title}
                                className='w-full'
                            />
                        ) : (
                            <Skeleton
                                className='h-[18.75rem] w-full scale-100'
                                variant='rectangular'
                            />
                        )}
                        <Box className='absolute bottom-0 left-6 h-px w-14 bg-primary-main' />
                    </Grid>
                    <Grid
                        item
                        xs={6}
                        sm={8}
                    >
                        <Stack className='w-full gap-11'>
                            <Stack className='flex-row gap-6 rounded-md bg-white px-4 py-3 md:px-6 md:py-5'>
                                <Stack className='gap-2'>
                                    {stopedLoadingWithData ? (
                                        <>
                                            <Typography className='text-8xl font-medium leading-[4rem]'>
                                                {Math.round(
                                                    detailData?.popularity,
                                                )}
                                                %
                                            </Typography>
                                            <Stack>
                                                <Rating
                                                    value={
                                                        detailData?.vote_average /
                                                        2
                                                    }
                                                    precision={0.5}
                                                    readOnly
                                                />
                                                <Typography>
                                                    total:{' '}
                                                    {detailData?.vote_count}
                                                </Typography>
                                            </Stack>
                                        </>
                                    ) : (
                                        <>
                                            <Skeleton className='h-24 w-44 scale-100' />
                                            <Stack>
                                                <Skeleton className='scale-80 h-9 w-44' />
                                                <Skeleton className='scale-80 h-9 w-40' />
                                            </Stack>
                                        </>
                                    )}
                                </Stack>
                                <Stack className='gap-6'>
                                    <Stack className='gap-3 sm:flex-row'>
                                        <Stack className='flex-row items-center gap-1'>
                                            <CalendarToday fontSize='small' />
                                            <Typography>
                                                {detailData?.release_date.substring(
                                                    0,
                                                    4,
                                                )}
                                            </Typography>
                                        </Stack>
                                        <Stack className='flex-row items-center gap-1'>
                                            <Language fontSize='small' />
                                            <Typography>
                                                {detailData?.original_language}
                                            </Typography>
                                        </Stack>
                                        <Stack className='flex-row items-center gap-1'>
                                            <AccessTime fontSize='small' />
                                            <Typography>
                                                {detailData?.runtime}min
                                            </Typography>
                                        </Stack>
                                    </Stack>
                                    <Stack className='hidden flex-row gap-2 sm:flex'>
                                        {detailData?.genres.map((genre) => (
                                            <Chip
                                                key={genre.id}
                                                label={genre.name}
                                                className='rounded-lg text-base'
                                            />
                                        ))}
                                    </Stack>
                                </Stack>
                            </Stack>
                            <Box>
                                <Typography
                                    variant='h1'
                                    className='pb-2 text-4xl font-medium'
                                >
                                    {detailData?.title}
                                </Typography>
                                <Typography className='text-base'>
                                    {detailData?.overview}
                                </Typography>
                            </Box>
                        </Stack>
                    </Grid>
                </Grid>
                <Box className='w-full items-center gap-4 text-center'>
                    <Typography
                        variant='h1'
                        className='text-6xl font-medium text-secondary-main md:text-7xl'
                    >
                        Movie Detail
                    </Typography>
                </Box>
                <Box className='flex justify-center'>
                    <Box className='overflow-hidden rounded-md border-solid border-primary-main'>
                        <Box className='p-4'>
                            <Box className='w-12 text-base' />
                            <Box className='text-base' />
                        </Box>
                    </Box>
                </Box>
            </Box>
        </Box>
    )
}
