import { useNavigate } from 'react-router-dom'

import { CircularProgress, Typography } from '@mui/material'

import ErrorMessage from '@/components/common/ErrorMessage.tsx'
import MovieTile from '@/components/movie/MovieTile.tsx'
import SearchMovieContent from '@/components/search/SearchMovieContent.tsx'
import { useMovieDetail } from '@/hooks/movies/useMovieDetail.ts'
import { useSimilarMovies } from '@/hooks/movies/useSimilarMovies.ts'
import { paths } from '@/paths.ts'

const MovieDetailPage = () => {
    const navigate = useNavigate()
    const { data, isError, isLoading } = useMovieDetail()
    const {
        data: similarMoviesData,
        isError: similarMoviesError,
        isLoading: isSimilarMoviesLoading,
    } = useSimilarMovies()

    if (isLoading) {
        return <CircularProgress size={100} sx={{ my: 10 }} />
    }

    if (isError) {
        return <ErrorMessage handleClear={() => navigate(paths.homepage)} />
    }

    return data ? (
        <>
            <MovieTile isDetail {...data} />

            <Typography variant="h2" sx={{ mt: 10, mb: 5 }}>
                You might also like
            </Typography>
            <SearchMovieContent
                // give 6 alternatives
                results={similarMoviesData?.slice(0, 6)}
                handleClear={() => navigate(paths.homepage)}
                isError={similarMoviesError}
                isLoading={isSimilarMoviesLoading}
            />
        </>
    ) : (
        <ErrorMessage
            error="We could not match any of the movie to the requested id."
            handleClear={() => navigate(paths.homepage)}
        />
    )
}

export default MovieDetailPage
