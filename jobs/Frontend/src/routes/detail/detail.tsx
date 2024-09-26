import { useParams } from '@tanstack/react-router'
import css from './detail.module.css'
import MovieDetail from './movie-detail/movie-detail'
import { Link } from '@tanstack/react-router'

const Detail = () => {
  const { movieId } = useParams({ from: '/detail/$movieId' })

  return (
    <div className={css.wrapper}>
      <div className={css.nav}>
        <div className={css.inner}>
          <Link to="/">⇦</Link>
        </div>
      </div>
      <div className={css.detail}>
        <div className={css.inner}>
          <MovieDetail movieId={movieId} />
        </div>
      </div>
    </div>
  )
}

export default Detail
