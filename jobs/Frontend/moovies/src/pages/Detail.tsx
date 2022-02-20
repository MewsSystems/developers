import styled from 'styled-components'
import { useEffect, useState } from "react"
import { useParams, Link } from "react-router-dom"
import useMovieDetail from "../hooks/useMovieDetail"
import { MovieObject } from "../components/SearchResultsList"
import Spinner from "../components/Spinner"
import StyledSection from '../components/utils/StyledSection'

const StyledLinkWrap = styled.div`
    min-height: 4rem;
    display: flex;
    align-items: center;
`

const StyledLink = styled(Link)`
    font-weight: 700;
    text-decoration: none;
    align-self: center;
    display: block;
    background-color: transparent;
    margin-left: 1.5rem;
    padding: 1rem;
    color: black;
    border: 1px solid transparent;
    transition: all 300ms ease-out;

    &:hover {
        background-color: #F8F8F8;
        border: 1px solid #353535;
    }
  `

const H1 = styled.div`
    font-size: 2rem;
    line-height: 2;
`

const Img = styled.img`
    height: 500px;
    width: auto;
    margin-left: 1.5rem;
    margin-right: auto;
`

const P = styled.p`
    font-size: 1rem;
    line-height: 1.5;
`

const GenreSpan = styled.span`
    margin-right: 1rem;
    font-size: 0.95rem;
`

const GenresWrap = styled.div`
    margin: 0 0 1rem 0;
`

const BasicInfo = styled.div`
    display: flex;
    flex-direction: column;
    justify-content: flex-start;
    margin: 1rem 1.5rem;
    color: #f8f8f8;
`

const H2 = styled.h2`
    font-size: 2rem;
    line-height: 2;
`

const StyledGrid = styled.div`
    display: grid;
    grid-template-rows: 1fr;
    grid-template-columns: 1fr;
    justify-content: center;


    @media only screen and (min-width: 700px) {
        grid-template-columns: min-content 1fr;
    }

    @media only screen and (min-width: 1100px) {
        grid-template-columns: 1fr 1fr 1fr;
    }   
`

type DetailParams = {
    movieId: string
}

const Detail = (props: any) => {

    let { movieId } = useParams<DetailParams>()

    const [movieData, setMovieData] = useState<MovieObject>()
    // const [isLoading, setIsLoading] = useState(true)
    const result = useMovieDetail(movieId)

    useEffect(() => {
        setMovieData(result.data[0])
        // setIsLoading(result.loading)
    })

    if (result.loading) {
        return (
            <Spinner />
        )
    }

    if (result.error || movieData === undefined) {
        return (
            <>
                <Link to={"/"}>Back to search</Link>
                <p>Oops. Couldnt find any movie with this ID.</p>
            </>
        )
    }

    const genres = movieData.genres.map((el) => <GenreSpan>{el.name}</GenreSpan>)

    return (
        <>
            <StyledLinkWrap>
                <StyledLink to={"/"}>Back to search results</StyledLink>
            </StyledLinkWrap>
            <StyledSection bgrCol='#353535'>
                <StyledGrid>
                    <Img src={`https://image.tmdb.org/t/p/w500/${movieData.poster_path}`} />
                    <BasicInfo>
                        <H1>{movieData.title}<span>{` (${parseInt(movieData.release_date, 10)}`})</span></H1>
                        <GenresWrap>
                            {genres}
                        </GenresWrap>
                        <P>{movieData.overview}</P>
                        <p>{movieData.tagline}</p>
                    </BasicInfo>
                </StyledGrid>
            </StyledSection>
            <StyledSection bgrCol='white'>
                <H2>Cast</H2>
                <H2>Budget</H2>
            </StyledSection>
        </>

    )
}

export default Detail
