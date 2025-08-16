import { useState } from 'react'
import css from './main.module.css'
import Movies from './movies/movies'
import Search from './search/search'
import { Outlet, useMatchRoute } from '@tanstack/react-router'

const Main = () => {
  const [query, setQuery] = useState('')
  const matchRoute = useMatchRoute()
  const showDetail = !!matchRoute({ to: '/detail/$movieId' })
  return (
    <div className={css.wrapper}>
      <div className={css.search}>
        <div className={css.inner}>
          <Search onChange={setQuery} />
        </div>
      </div>
      <div className={css.movies}>
        <div className={css.inner}>
          <Movies query={query} />
        </div>
      </div>
      {showDetail ? (
        <div className={css.detail}>
          <Outlet />
        </div>
      ) : null}
    </div>
  )
}

export default Main
