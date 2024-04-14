import { Grid, Stack, Skeleton, Typography } from '@mui/material'
import type { PosterContentProps } from '../types'
import { MovieThumbnail } from './MovieThumbnail'

export const PostersContent = (props: PosterContentProps) => {
    const {
        searchData,
        isLoading,
        isError,
        prefetchFunction,
        errorText,
        noDataText,
        limitMovies = false,
    } = props

    const movieItems = limitMovies
        ? searchData?.results?.slice(0, 3)
        : searchData?.results

    return (
        <Grid
            container
            spacing={2}
            className='mb-10'
        >
            {isLoading ? (
                [...Array(3)].map((_, index) => (
                    <Grid
                        item
                        xs={6}
                        md={4}
                        key={index}
                    >
                        <Stack className='overflow-hidden rounded-md border-solid border-primary-main'>
                            <Skeleton
                                className='h-[18.75rem] scale-100'
                                variant='rectangular'
                            />
                            <Stack className='p-4'>
                                <Skeleton className='w-12 text-base' />
                                <Skeleton className='text-base' />
                            </Stack>
                        </Stack>
                    </Grid>
                ))
            ) : isError ? (
                <Grid
                    item
                    xs={12}
                >
                    <Stack className='w-full flex-row items-center justify-center rounded-md bg-gray-100 py-7'>
                        <Typography>{errorText}</Typography>
                    </Stack>
                </Grid>
            ) : searchData?.results.length === 0 ? (
                <Grid
                    item
                    xs={12}
                >
                    <Stack className='w-full flex-row items-center justify-center rounded-md bg-gray-100 py-7'>
                        <Typography>{noDataText}</Typography>
                    </Stack>
                </Grid>
            ) : (
                movieItems?.map((thumbnailItem) => (
                    <Grid
                        item
                        xs={6}
                        md={4}
                        key={thumbnailItem.id}
                    >
                        <MovieThumbnail
                            onMouseEnter={() =>
                                prefetchFunction(thumbnailItem.id)
                            }
                            {...thumbnailItem}
                        />
                    </Grid>
                ))
            )}
        </Grid>
    )
}
