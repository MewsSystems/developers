import { useMovie } from 'data/movies/movies'
import Rating from 'components/rating/rating'
import css from './movie-detail.module.css'
import Error from './states/error'
import Pending from './states/pending'

export interface MovieDetailProps {
  movieId: string
}

const joinByKey = <T, K extends keyof T>(source: T[], key: K): string => {
  return source
    .map((item) => {
      return item[key]
    })
    .join(', ')
}

const POSTER_URL = 'https://image.tmdb.org/t/p/w220_and_h330_face'

const MovieDetail = ({ movieId }: MovieDetailProps) => {
  const { data, isPending, error } = useMovie(movieId)

  if (isPending) return <Pending />
  if (error) return <Error />

  return (
    <div className={css.wrapper}>
      <div className={css.poster}>
        {data.poster_path ? (
          <img
            loading="lazy"
            src={POSTER_URL + data.poster_path}
            width="220"
            height="330"
          />
        ) : (
          <div className={css.noImage} />
        )}
      </div>
      <div className={css.movieInfo}>
        <div className={css.title}>{data.title}</div>
        <div className={css.rating}>
          <Rating rating={data.vote_average} />
        </div>
        <div className={css.overview}>{data.overview}</div>
        <table className={css.table}>
          <tbody>
            <tr>
              <th>Release date</th>
              <td>{data.release_date}</td>
            </tr>
            <tr>
              <th>Genres</th>
              <td>{joinByKey(data.genres, 'name')}</td>
            </tr>
            <tr>
              <th>Countries</th>
              <td>{joinByKey(data.production_countries, 'name')}</td>
            </tr>
            <tr>
              <th>Languages</th>
              <td>{joinByKey(data.spoken_languages, 'english_name')}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  )
}

export default MovieDetail
