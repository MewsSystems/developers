import React, { useEffect, useState, useCallback } from 'react'
import { RouteComponentProps, navigate } from '@reach/router'
import { SceneMetaProps, SceneMeta } from 'components/SceneMeta'
import { Heading } from 'components/Heading'
import { useTranslation } from 'react-i18next'
import { useThunkDispatch } from 'hooks/useThunkDispatch'
import { hydrateTrendingMovies } from 'state/actions/movies'
import { useSelector } from 'react-redux'
import { State } from 'state/rootReducer'
import { differenceInDays } from 'date-fns'
import styled from 'styled-components'
import { MovieCard } from 'components/MovieCard'
import { Movie } from 'model/api/Movie'
import { Button } from 'components/Button'
import { useGetImagePath } from 'hooks/useGetImagePath'
import { ConfigurationBackdropSizesEnum } from 'model/api/ConfigurationBackdropSizesEnum'
import { Card as LoadingCard } from 'components/Loading/Card'
import { Spacing } from 'components/Spacing'
import { Content } from 'components/Content'

const Grid = styled.div`
  display: grid;
  grid-gap: 1rem;
  grid-template-columns: 1fr 1fr 1fr;
`

const { PUBLIC_URL } = process.env

export const Home: React.FC<RouteComponentProps<SceneMetaProps>> = ({
  title,
  description,
}) => {
  const { t } = useTranslation('home')
  const dispatch = useThunkDispatch()
  const [trendingMovies, setTrendingMovies] = useState<Movie[]>([])
  const [page, setPage] = useState(1)
  const {
    updatedAt,
    trending: { results, total_pages },
    loading,
  } = useSelector((state: State) => state.movies)
  const getImagePath = useGetImagePath(ConfigurationBackdropSizesEnum.W_780)
  const dayDifference = differenceInDays(new Date(), updatedAt)

  const handleLoadMore = useCallback(() => {
    setPage(page + 1)
  }, [page])

  const reduceTrendingMovies = useCallback(() => {
    return Object.keys(results).reduce<Movie[]>((acc, index) => {
      if (Number(index) <= page) {
        acc = [...acc, ...results[index]]
      }

      return acc
    }, [])
  }, [results, page])

  const handleCardClick = useCallback((id: number) => {
    navigate(`${PUBLIC_URL}/movie/${id}`)
  }, [])

  useEffect(() => {
    if (dayDifference > 7) {
      dispatch(hydrateTrendingMovies(true))
    } else {
      dispatch(hydrateTrendingMovies(false, page))
    }
  }, [dispatch, dayDifference, page])

  useEffect(() => {
    setTrendingMovies(reduceTrendingMovies())
  }, [page, results, reduceTrendingMovies])

  return (
    <>
      <SceneMeta title={title} description={description} />
      <Content>
        <Heading level={1}>{t('trending')}</Heading>
        {trendingMovies.length > 0 && (
          <>
            <Grid>
              {trendingMovies.map(
                ({ title, overview, backdrop_path, original_language, id }) => (
                  <MovieCard
                    key={id}
                    id={id}
                    title={title}
                    overview={overview}
                    background={getImagePath(backdrop_path)}
                    language={original_language}
                    onClick={handleCardClick}
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
                onClick={handleLoadMore}
                disabled={page === total_pages || loading}
                fullWidth
              >
                {t('load-more')}
              </Button>
            </Spacing>
          </>
        )}
      </Content>
    </>
  )
}
