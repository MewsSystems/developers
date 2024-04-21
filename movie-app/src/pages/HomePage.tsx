import { useNavigate } from 'react-router-dom'

import { Pagination, TextField, Typography } from '@mui/material'

import SearchMovieContent from '@/components/search/SearchMovieContent.tsx'
import { useSearchMovie } from '@/hooks/movies/useSearchMovie.ts'

const HomePage = () => {
    const navigate = useNavigate()
    const {
        currentPage,
        handlePageChange,
        handleSearchChange,
        isError,
        isLoading,
        results,
        searchQuery,
        searchRef,
        totalPages,
    } = useSearchMovie()

    return (
        <div>
            <Typography variant="h1">Lights, Camera, Search!</Typography>
            <Typography variant="body1" sx={{ my: 5 }}>
                Hunting for the perfect movie has never been easier! Simply
                enter keywords into the search bar and let our app work its
                magic. Start your journey now and unlock a universe of cinematic
                wonders
            </Typography>
            <TextField
                aria-label="Search movie"
                fullWidth
                helperText={
                    searchQuery?.length < 3
                        ? 'You need to enter at least 3 characters to start search'
                        : ''
                }
                hiddenLabel
                inputProps={{ 'data-testid': 'search' }}
                inputRef={searchRef}
                name="search"
                onChange={(event) => handleSearchChange(event.target.value)}
            />
            <>
                <SearchMovieContent
                    isLoading={isLoading}
                    isError={isError}
                    results={results}
                    handleClear={() => navigate('/', { replace: true })}
                />

                {!!results?.length && (
                    <Pagination
                        color="primary"
                        count={totalPages}
                        onChange={handlePageChange}
                        page={currentPage}
                        sx={{
                            display: 'inline-flex',
                            mx: { xs: -2, md: 'auto' },
                        }}
                    />
                )}
            </>
        </div>
    )
}

export default HomePage
