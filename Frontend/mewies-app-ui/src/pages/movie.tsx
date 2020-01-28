import React from 'react'
import Head from '../components/Head/Head'
import { Col, Container, Row } from '../utils/layout/grid.layout'
import { NextPage } from 'next'
import { MovieModel } from '../utils/types/model'
import { initialMovieDetails } from '../utils/constants/movie.constants'
import { getMovieById } from '../utils/api/movie.requests'
import { mapMovieDtoToModel } from '../utils/mappers/movie.mappers'
import { useRouter } from 'next/router'
import { Button } from '../components/Button/Button'
import { formatReleaseDate } from '../utils/helpers/component.helpers'
import { GenreDto } from '../utils/types/dto'
import {
    MovieDetailsSection,
    MovieDetailsContent,
    MovieDetailsGenres,
    MovieHeader,
} from '../components/Movie/Movie.styles'
import { AppRoutes } from '../utils/constants/routes.constants'
import { serverRedirect } from '../utils/helpers/server.helpers'

interface MovieProps extends MovieModel {}

interface GenreTagProps extends Omit<GenreDto, 'id'> {}

const Movie: NextPage<MovieProps> = props => {
    const router = useRouter()

    return (
        <Container>
            <Head
                url={''}
                ogImage={''}
                title={props.title}
                description={props.overview}
            />
            <Row>
                <Col size={12}>
                    <MovieHeader>
                        <nav>
                            <Button onClick={() => router.push(AppRoutes.Home)}>
                                Go back
                            </Button>
                        </nav>
                    </MovieHeader>
                    <MovieDetailsSection>
                        <div>
                            <img
                                height={540}
                                src={
                                    'https://image.tmdb.org/t/p/original' +
                                    props.posterPath
                                }
                                alt={props.title}
                            />
                        </div>
                        <MovieDetailsContent>
                            <h1>{props.title}</h1>
                            <p>{formatReleaseDate(props.releaseDate)}</p>
                            <MovieDetailsGenres>
                                {props.genres.map(({ name }) => (
                                    <GenreTag name={name} />
                                ))}
                            </MovieDetailsGenres>
                            <h3>Overview</h3>
                            <p>{props.overview}</p>
                            <p>
                                Average rating:{' '}
                                <strong>{props.voteAverage}</strong>
                            </p>
                        </MovieDetailsContent>
                    </MovieDetailsSection>
                </Col>
            </Row>
        </Container>
    )
}

const GenreTag: React.FC<GenreTagProps> = ({ name }) => <li>{name}</li>

Movie.getInitialProps = async ctx => {
    try {
        const { id } = ctx.query
        const movie = await getMovieById(String(id))
        return mapMovieDtoToModel(movie)
    } catch (e) {
        serverRedirect(ctx, AppRoutes.Home)
        return initialMovieDetails
    }
}

export default Movie
