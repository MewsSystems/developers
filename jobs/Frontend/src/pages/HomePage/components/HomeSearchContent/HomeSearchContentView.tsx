import {
    Box,
    Pagination,
    Typography,
    Stack,
    Grid,
    Skeleton,
} from '@mui/material'
import { FC } from 'react'
import { HomeSearchContentProps } from '../../types'
import { MovieThumbnail } from '../../../../components'

export const HomeSearchContentView: FC<HomeSearchContentProps> = (props) => {
    const {
        isLoading,
        isError,
        handleChangePage,
        searchData,
        prefetchMovieData,
    } = props

    return (
        <Stack
            component='section'
            className='container'
        >
            {isLoading ? (
                <Grid
                    container
                    spacing={2}
                    className='mb-10'
                >
                    {[...Array(3)].map((_, index) => (
                        <Grid
                            item
                            xs={6}
                            md={4}
                            key={index}
                        >
                            <Stack className='overflow-hidden rounded-md border-solid border-primary-main'>
                                <Skeleton
                                    height={300}
                                    className='scale-100'
                                    variant='rectangular'
                                />
                                <Stack className='p-4'>
                                    <Skeleton className='w-12 text-base' />
                                    <Skeleton className='text-base' />
                                </Stack>
                            </Stack>
                        </Grid>
                    ))}
                </Grid>
            ) : isError ? (
                <Typography>Error...</Typography>
            ) : !!searchData?.results && searchData?.results.length > 0 ? (
                <>
                    <Grid
                        container
                        spacing={2}
                        className='mb-10'
                    >
                        {searchData?.results?.map((thumbnailItem) => (
                            <Grid
                                item
                                xs={6}
                                md={4}
                                key={thumbnailItem.id}
                            >
                                <MovieThumbnail
                                    onMouseEnter={() =>
                                        prefetchMovieData(thumbnailItem.id)
                                    }
                                    {...thumbnailItem}
                                />
                            </Grid>
                        ))}
                    </Grid>
                    <Box className='mx-auto'>
                        <Pagination
                            count={searchData?.total_pages}
                            variant='outlined'
                            onChange={handleChangePage}
                        />
                    </Box>
                </>
            ) : (
                <Stack></Stack>
            )}
        </Stack>
    )
}
