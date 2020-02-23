import React, { useState, useCallback, useEffect } from 'react'
import { SceneMeta, SceneMetaProps } from 'components/SceneMeta'
import { State } from 'state/rootReducer'
import { useSelector } from 'react-redux'
import { RouteComponentProps, navigate } from '@reach/router'
import { Content } from 'components/Content'
import { Heading } from 'components/Heading'
import { useTranslation } from 'react-i18next'
import { COLORS } from 'constants/colors'
import styled from 'styled-components'
import { MovieList } from 'components/MovieList'
import { Movie } from 'model/api/Movie'
import { useGetSearchResults } from 'hooks/useGetSearchResults'
import { Spacing } from 'components/Spacing'
import { ErrorContent } from 'components/ErrorContent'
import { Loading } from './components/Loading'

const { PUBLIC_URL } = process.env

const SearchQuery = styled.span`
  color: ${COLORS.DARK_GRAY};
  font-weight: bold;
`
export const Search: React.FC<RouteComponentProps<SceneMetaProps>> = ({
  title,
  description,
}) => {
  const { t } = useTranslation('search')
  const [movies, setMovies] = useState<Movie[]>([])
  const [page, setPage] = useState(1)
  const [totalPages, setTotalPages] = useState(0)
  const searchValue = useSelector((state: State) => state.search.value)
  const { results, isLoading, hasErrored } = useGetSearchResults(page)

  const handleLoadMore = useCallback(() => {
    setPage(page + 1)
  }, [page])

  const handleMovieClick = useCallback((id: number) => {
    navigate(`${PUBLIC_URL}/movie/${id}`)
  }, [])

  useEffect(() => {
    setMovies(state => [...state, ...results.results])
    setTotalPages(results.total_pages)
  }, [results])

  if (hasErrored)
    return (
      <ErrorContent
        title={t('error-content.title')}
        text={t('error-content.text')}
      />
    )

  if (isLoading) return <Loading />

  return (
    <>
      <SceneMeta title={`${title} ${searchValue}`} description={description} />
      <Content>
        <Heading level={1}>{t('search-results')}</Heading>
        {searchValue.length > 0 ? (
          <>
            <Spacing outer="0 0 2rem">
              {t('searching-for')} <SearchQuery>{searchValue}</SearchQuery>
            </Spacing>

            {results.total_results === 0 ? (
              <Heading level={2}>
                {t('no-search-results', { searchValue })}
              </Heading>
            ) : (
              <MovieList
                items={movies}
                onMovieClick={handleMovieClick}
                onLoadMore={handleLoadMore}
                page={page}
                totalPages={totalPages}
                loading={isLoading}
              />
            )}
          </>
        ) : (
          <Heading level={2}>{t('no-search-value')}</Heading>
        )}
      </Content>
    </>
  )
}
