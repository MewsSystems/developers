import { Box, Pagination, Stack } from '@mui/material'
import type { FC } from 'react'
import type { HomeSearchContentProps } from '../../types'
import { PostersContent } from '../../../../components'

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
            role='region'
        >
            <PostersContent
                searchData={searchData}
                isLoading={isLoading}
                isError={isError}
                errorText='There has been an error with the search. Please try again.'
                noDataText="There doesn't seem to be any movie with that title. Please try another."
                prefetchFunction={prefetchMovieData}
            />
            {!(isLoading || isError || searchData?.results.length === 0) ? (
                <Box className='mx-auto'>
                    <Pagination
                        size='medium'
                        siblingCount={0}
                        count={searchData?.total_pages}
                        variant='outlined'
                        onChange={handleChangePage}
                    />
                </Box>
            ) : null}
        </Stack>
    )
}
