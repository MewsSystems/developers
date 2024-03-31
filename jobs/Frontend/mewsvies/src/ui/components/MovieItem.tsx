import { Movie } from '../../domain/interfaces/movie.interface.ts'
import { IMAGE_BASE_URL } from '../constants/movie.constant.ts'
import styles from './MovieItem.module.scss'

interface MovieItemProps {
    movie: Movie
}

export function MovieItem({ movie }: MovieItemProps) {
    const posterPath = `${IMAGE_BASE_URL}${movie.poster_path}`
    const releaseDate = new Date(movie.release_date).toLocaleDateString()

    return (
        <div className={styles.movie}>
            <div className={styles['image-container']}>
                <img src={posterPath} alt={movie.title} />
            </div>
            <div className={styles['movie-info']}>
                <div>
                    <h3>{movie.title}</h3>
                    <p className={styles.resume}>{movie.overview}</p>
                </div>
                <strong>Release date: {releaseDate}</strong>
            </div>
        </div>
    )
}
