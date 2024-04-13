import {
    Box,
    Pagination,
    Typography,
    Stack,
    Grid,
    Divider,
} from '@mui/material'
import fallbackImageSrc from '../../../../assets/fallback-image.png'
import { FC } from 'react'
import { HomeSearchContentProps } from '../../types'

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
                <Typography>Loading...</Typography>
            ) : isError ? (
                <Typography>Error...</Typography>
            ) : !!searchData?.results && searchData?.results.length > 0 ? (
                <>
                    <Grid
                        container
                        spacing={2}
                        className='mb-10'
                    >
                        {searchData?.results?.map(
                            ({ id, title, release_date, poster_path }) => (
                                <Grid
                                    item
                                    xs={6}
                                    md={4}
                                    key={id}
                                >
                                    <Stack
                                        className=' h-full overflow-hidden rounded-md border-solid border-primary-main'
                                        onMouseEnter={() =>
                                            prefetchMovieData(id)
                                        }
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
                                        <Stack className=' p-4'>
                                            {release_date && (
                                                <Typography>
                                                    {release_date.substring(
                                                        0,
                                                        4,
                                                    )}
                                                </Typography>
                                            )}
                                            <Typography className='font-medium'>
                                                {title}
                                            </Typography>
                                        </Stack>
                                    </Stack>
                                </Grid>
                            ),
                        )}
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
