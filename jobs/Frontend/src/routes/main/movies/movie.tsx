import { Link } from '@tanstack/react-router'
import Rating from 'components/rating/rating'
import type { MovieInfo } from 'data/movies/movies'
import css from './movies.module.css'

interface MovieProps {
  data: MovieInfo
}

const POSTER_URL = 'https://image.tmdb.org/t/p/w220_and_h330_face'

const Movie = ({ data }: MovieProps) => {
  return (
    <Link
      className={css.movie}
      to="/detail/$movieId"
      params={{ movieId: String(data.id) }}
    >
      <div className={css.poster}>
        {data.poster_path ? (
          <img
            loading="lazy"
            src={POSTER_URL + data.poster_path}
            width="110"
            height="165"
          />
        ) : (
          <div className={css.noImage} />
        )}
      </div>
      <div className={css.movieInfo}>
        <div className={css.title} title={data.title}>
          {data.title}
        </div>
        <div className={css.subInfo}>
          <div className={css.date}>{data.release_date}</div>
          <Rating rating={data.vote_average} size="tiny" />
        </div>
      </div>
    </Link>
  )
}

export default Movie
