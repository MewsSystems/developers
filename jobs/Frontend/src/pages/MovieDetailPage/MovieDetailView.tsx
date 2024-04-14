import type { MovieDetailPageProps } from './types'
import type { FC } from 'react'
import { Box, Button, Grid, Skeleton, Stack, Typography } from '@mui/material'
import { ArrowBack, Home } from '@mui/icons-material'
import { MoviePoster, PostersContent } from '../../components'
import { TopSummary } from './components'

export const MovieDetailView: FC<MovieDetailPageProps> = (props) => {
    const {
        detailData,
        isLoading,
        isError,
        navigateBack,
        navigateHome,
        isPreviousPageAvailable,
        similarMovieData,
        isSimilarLoading,
        isSimilarError,
        prefetchSimilarMovieData,
    } = props

    const stopedLoadingWithData = !isLoading && !!detailData

    return (
        <Box className='relative bg-gradient-to-b from-gray-200 to-white bg-[length:100%_40rem] bg-no-repeat pb-7'>
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
                {isError ? (
                    <Stack className='mb-6 w-full flex-row items-center justify-center rounded-md bg-gray-100 py-7'>
                        <Typography>
                            There's been an error with loading data for the
                            selected movie. Please try again.
                        </Typography>
                    </Stack>
                ) : (
                    <Grid
                        container
                        spacing={3}
                        className='mb-10'
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
                            {detailData?.overview && (
                                <Box className='absolute bottom-0 left-6 h-px w-14 bg-primary-main' />
                            )}
                        </Grid>
                        <Grid
                            item
                            xs={6}
                            sm={8}
                        >
                            <Stack className='w-full gap-6'>
                                <TopSummary
                                    detailData={detailData}
                                    isLoading={isLoading}
                                />
                            </Stack>
                        </Grid>
                    </Grid>
                )}
                <Stack className='gap-6'>
                    <Typography
                        variant='h2'
                        className='text-2xl font-medium'
                    >
                        You might also like
                    </Typography>
                    <PostersContent
                        limitMovies
                        searchData={similarMovieData}
                        isLoading={isSimilarLoading}
                        isError={isSimilarError}
                        errorText="Seems there's been an error with finding similar movies, please try it again later."
                        noDataText="Seems there's no similar title to this one."
                        prefetchFunction={prefetchSimilarMovieData}
                    />
                </Stack>
            </Box>
        </Box>
    )
}
