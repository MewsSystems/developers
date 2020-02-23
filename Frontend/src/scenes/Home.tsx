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
import { Movie } from 'model/api/Movie'
import { Content } from 'components/Content'
import { MovieList } from 'components/MovieList'

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
  const dayDifference = differenceInDays(new Date(), new Date(updatedAt))

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

  const handleMovieClick = useCallback((id: number) => {
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
        <MovieList
          items={trendingMovies}
          page={page}
          totalPages={total_pages}
          loading={loading}
          onLoadMore={handleLoadMore}
          onMovieClick={handleMovieClick}
        />
      </Content>
    </>
  )
}
