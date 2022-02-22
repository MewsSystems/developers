import styled from 'styled-components'
import { useEffect, useState } from "react"
import { useParams, Link } from "react-router-dom"
import useMovieDetail from "../hooks/useMovieDetail"
import { MovieObject } from "../components/SearchResultsList"
import Spinner from "../components/Spinner"
import StyledSection from '../components/utils/StyledSection'
import CastInfoBox from '../components/CastInfoBox'
import { H1 } from '../components/utils/Typography'
import ImgPlaceholder from '../components/utils/ImgPlaceholder'

const StyledLinkWrap = styled.div`
    min-height: 4rem;
    display: flex;
    align-items: center;
    background-color: #F8F8F8;
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
        background-color: #d8d8d8;
        border: 1px solid #353535;
    }
  `

const Img = styled.img`
    height: 500px;
    width: auto;
`

const ImgWrap = styled.div`
    margin-right: auto;
`

const P = styled.p`
    font-size: 1rem;
    line-height: 1.5;
`

const GenreSpan = styled.span`
    margin-right: 1rem;
    font-size: 0.9rem;
`

const GenresWrap = styled.div`
    margin: 1rem 0 1rem 0;
`

const BasicInfo = styled.div`
    display: flex;
    flex-direction: column;
    justify-content: flex-start;
    padding: 1rem 1.5rem;
    color: #f8f8f8;
`

const LayoutGrid = styled.div`
    display: grid;
    grid-template-rows: 1fr;
    grid-template-columns: 1fr;
    justify-content: center;
    gap: 0 0;

    @media only screen and (min-width: 700px) {
        grid-template-columns: min-content 1fr;
        gap: 0rem 0.5rem;
    }

    @media only screen and (min-width: 1100px) {
        grid-template-columns: min-content 1fr 10%;
        gap: 0rem 1rem;
    }   
`

type DetailParams = {
    movieId: string
}

const Detail = (props: any) => {

    let { movieId } = useParams<DetailParams>()
    const [movieData, setMovieData] = useState<MovieObject>()
    const result = useMovieDetail(movieId)

    useEffect(() => {
        setMovieData(result.data[0])
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

    const genres = movieData.genres.map((el) => <GenreSpan key={el.id}>{el.name}</GenreSpan>)

    return (
        <>
            <StyledLinkWrap>
                <StyledLink to={"/"}>Back to search results</StyledLink>
            </StyledLinkWrap>
            <StyledSection bgrCol='#353535'>
                <LayoutGrid>
                    <ImgWrap>
                        {movieData.poster_path
                            ?
                            <Img src={`https://image.tmdb.org/t/p/w500/${movieData.poster_path}`} />
                            :
                            <ImgPlaceholder height="500px" width="333px">
                                no image available
                            </ImgPlaceholder>
                        }
                    </ImgWrap>
                    <BasicInfo>
                        <H1>{movieData.title}<span>{` (${parseInt(movieData.release_date, 10)}`})</span></H1>
                        <GenresWrap>
                            {genres}
                        </GenresWrap>
                        <P>{movieData.overview}</P>
                        <P>{movieData.tagline}</P>
                    </BasicInfo>
                </LayoutGrid>
            </StyledSection>
            <StyledSection bgrCol='#f8f8f8'>
                <CastInfoBox cast={movieData.credits.cast} />
            </StyledSection>
        </>

    )
}

export default Detail
