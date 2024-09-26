import { useEffect, Fragment } from 'react'
import { useInView } from 'react-intersection-observer'
import { useMovies } from 'data/movies/movies'
import Grid from 'components/grid/grid'
import Loading from 'components/loading/loading'
import css from './movies.module.css'
import Movie from './movie'

import NoQuery from './states/no-query'
import Pending from './states/pending'
import NoResults from './states/no-results'
import Error from './states/error'

interface MoviesProps {
  query: string
}

const Movies = ({ query }: MoviesProps) => {
  const { ref, inView } = useInView()
  const result = useMovies(query)
  const {
    data,
    isPending,
    error,
    fetchNextPage,
    isFetchingNextPage,
    hasNextPage,
  } = result

  useEffect(() => {
    if (inView && hasNextPage) fetchNextPage()
  }, [fetchNextPage, hasNextPage, inView])

  if (!query) return <NoQuery />
  if (isPending) return <Pending />
  if (error) return <Error />
  if (data.pages.length === 1 && data.pages[0].results.length === 0) {
    return <NoResults />
  }

  const grids = data.pages.map((page, i) => {
    return (
      <Fragment key={page.page}>
        <Grid
          list={page.results}
          renderItem={(item) => <Movie data={item} />}
        />
        {data.pages.length > i + 1 ? <hr /> : null}
      </Fragment>
    )
  })

  const totalResults = data.pages[0].total_results

  return (
    <div className={css.wrapper}>
      <div className={css.totalResutls}>
        {totalResults} Result{totalResults !== 1 ? 's' : ''}
      </div>
      {grids}
      <div ref={ref} />
      <div>
        {isFetchingNextPage ? (
          <div className={css.fetching}>
            <Loading />
          </div>
        ) : null}
      </div>
    </div>
  )
}

export default Movies
