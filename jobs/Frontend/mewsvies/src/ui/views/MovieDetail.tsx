import { useParams } from 'react-router-dom'
import { useState, useEffect } from 'react'
import { getMovieDetail } from '../../domain/services/movie.service.ts'
import { MovieInformation } from '../../domain/interfaces/movie.interface.ts'
import styles from './MovieDetail.module.scss'
import { getImageURL } from '../utils/getImageURL.ts'
import { getInitialMovieInformation } from '../utils/getInitialMovieInformation.ts'

export default function MovieDetail() {
    const { id } = useParams()
    const [movie, setMovie] = useState<MovieInformation>(
        getInitialMovieInformation()
    )

    const posterPath = getImageURL(movie.poster_path)
    const releaseDate = new Date(movie.release_date).toLocaleDateString()
    const genres = movie.genres.map((genre) => genre.name).join(', ')
    const productionCompanies = movie.production_companies
        .map(({ name }) => name)
        .join(', ')

    useEffect(() => {
        if (!id) return

        const fetchMovieDetail = async () => {
            const data = await getMovieDetail(id)
            setMovie(data)
        }

        fetchMovieDetail()
    }, [id])

    return (
        <div className={styles.movie}>
            <div className={styles['image-container']}>
                <img src={posterPath} alt={movie.title} />
            </div>
            <div className={styles['movie-info']}>
                <div>
                    <h2>{movie.title}</h2>
                    <hr />
                    <h4>Summary:</h4>
                    <p>{movie.overview}</p>
                    <p>
                        <strong>Genre: </strong>
                        {genres}
                    </p>
                    <p>
                        <strong>Production Companies: </strong>
                        {productionCompanies}
                    </p>
                </div>
                <strong>Release date: {releaseDate}</strong>
            </div>
        </div>
    )
}
