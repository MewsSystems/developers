import { useNavigate, generatePath } from 'react-router-dom'
import { Movie } from '../../domain/interfaces/movie.interface.ts'
import styles from './MovieItem.module.scss'
import { getImageURL } from '../utils/getImageURL.ts'

interface MovieItemProps {
    movie: Movie
}

export default function MovieItem({ movie }: MovieItemProps) {
    const navigate = useNavigate()

    const posterPath = getImageURL(movie.poster_path)
    const releaseDate = new Date(movie.release_date).toLocaleDateString()

    function goToMovieDetail() {
        navigate(generatePath('/movie/:id', { id: `${movie.id}` }))
    }

    return (
        <div className={styles.movie} onClick={goToMovieDetail}>
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
