import { useNavigate } from 'react-router-dom'

import { Box, Button, Rating, Typography } from '@mui/material'

import MovieDetailInformation from './MovieDetailInformation.tsx'
import MovieImg from './MovieImg.tsx'

import { Movie } from '@/hooks/movies/types.ts'

type MovieTileProps = { isDetail?: boolean } & Movie

export const MovieTile = ({ isDetail = false, ...props }: MovieTileProps) => {
    const navigate = useNavigate()

    const {
        id,
        overview,
        poster_path: posterPath,
        title,
        vote_average: voteAverage,
        vote_count: voteCount,
        release_date: releaseDate,
        original_language: originalLanguage,
        original_title: originalTitle,
        tagline,
        genres,
    } = props

    // to clamp 3 lines in search
    const overviewSx = !isDetail
        ? {
              display: '-webkit-box',
              maxWidth: '100%',
              WebkitLineClamp: 3,
              WebkitBoxOrient: 'vertical',
              overflow: 'hidden',
              textOverflow: 'ellipsis',
          }
        : {}

    return (
        <Box
            sx={{
                color: '#F9F8FF',
                borderRadius: 5,
                backgroundColor: '#202238',
                p: 1,
                display: 'flex',
                flexWrap: { xs: 'wrap', md: 'nowrap' },
                justifyContent: { xs: 'center', md: 'flex-start' },
                alignItems: 'center',
                gap: 2,
                overflow: 'hidden',
                textAlign: { xs: 'center', md: 'left' },
            }}
        >
            <MovieImg
                title={title}
                posterPath={posterPath}
                isDetail={isDetail}
            />

            <div>
                {isDetail && (
                    <MovieDetailInformation
                        original_language={originalLanguage}
                        original_title={originalTitle}
                        release_date={releaseDate}
                        genres={genres}
                    />
                )}

                <Typography
                    data-testid="movie-title"
                    variant="h3"
                    sx={{ mt: 1 }}
                >
                    {title}
                </Typography>

                {isDetail && tagline && (
                    <Typography
                        data-testid="movie-tagline"
                        variant="body2"
                        sx={{ my: 1, mb: 2, fontStyle: 'italic' }}
                    >
                        {tagline}
                    </Typography>
                )}

                {voteAverage && (
                    <Box
                        sx={{
                            display: 'flex',
                            gap: 1,
                            alignItems: 'center',
                            justifyContent: { xs: 'center', md: 'flex-start' },
                        }}
                    >
                        <Rating
                            max={5}
                            precision={0.5}
                            name="Movie rating"
                            size="small"
                            value={voteAverage / 2}
                            readOnly
                        />
                        <Typography variant="body2" sx={{ fontSize: 12 }}>
                            ({voteCount || 0})
                        </Typography>
                    </Box>
                )}

                <Typography
                    data-testid="movie-overview"
                    variant="body2"
                    sx={{
                        my: 2,
                        ...overviewSx,
                    }}
                >
                    {overview}
                </Typography>

                {!isDetail && (
                    <Button
                        onClick={() => navigate(`/movie/${id}`)}
                        variant="contained"
                        sx={{
                            width: { xs: 1, md: 'auto' },
                            textTransform: 'initial',
                        }}
                    >
                        Go to detail
                    </Button>
                )}
            </div>
        </Box>
    )
}

export default MovieTile
