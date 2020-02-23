import React from 'react'
import { MovieCard } from './MovieCard'
import styled from 'styled-components'
import { Movie } from 'model/api/Movie'
import { Button } from './Button'
import { Spacing } from './Spacing'
import { Card as LoadingCard } from 'components/Loading/Card'
import { ConfigurationBackdropSizesEnum } from 'model/api/ConfigurationBackdropSizesEnum'
import { useTranslation } from 'react-i18next'
import { useGetImagePath } from 'hooks/useGetImagePath'

const Grid = styled.div`
  display: grid;
  grid-gap: 1rem;
  grid-template-columns: 1fr 1fr 1fr;
`

export interface MovieListProps {
  items: Movie[]
  page: number
  totalPages: number
  loading: boolean
  onLoadMore: () => void
  onMovieClick?: (id: number) => void
}

export const MovieList: React.FC<MovieListProps> = ({
  items,
  page,
  totalPages,
  loading,
  onLoadMore,
  onMovieClick,
}) => {
  const { t } = useTranslation()
  const getImagePath = useGetImagePath(ConfigurationBackdropSizesEnum.W_780)

  if (items.length > 0) {
    return (
      <>
        <Grid>
          {items.map(
            ({ title, overview, backdrop_path, original_language, id }) => (
              <MovieCard
                key={id}
                id={id}
                title={title}
                overview={overview}
                background={getImagePath(backdrop_path)}
                language={original_language}
                onClick={onMovieClick}
              />
            )
          )}
          {loading && (
            <>
              <LoadingCard />
              <LoadingCard />
              <LoadingCard />
              <LoadingCard />
              <LoadingCard />
            </>
          )}
        </Grid>
        <Spacing outer="2rem 0 0">
          <Button
            onClick={onLoadMore}
            disabled={page === totalPages || loading}
            fullWidth
          >
            {t('load-more')}
          </Button>
        </Spacing>
      </>
    )
  }

  return null
}
