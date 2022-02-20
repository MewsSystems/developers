import styled from 'styled-components'
import { Link } from 'react-router-dom'

const StyledListItem = styled.div`
    display: flex;
    background-color: #f8f8f8;
    border-radius: 0.25rem;
    transition: background-color 200ms ease-out;
    &:hover {
        background-color: #ebf2fa;
    }
`

const StyledInfoBox = styled.div`
    display: flex;
    flex-direction: column;
    padding: 1rem;
    width: 100%;
`

const Img = styled.img`
    object-fit: cover;
    height: 150px;
`

const H3 = styled.h3`
    font-size: 1rem;
    font-weight: 400;
    line-heigth: 1.5;
    display: inline-block;
`

const P = styled.p`
    margin-top: 0.25rem;
    font-size: 0.85rem;
    line-height: 1.3;
    color: black;
    min-width: 200px;
`

const StyledLink = styled(Link)`
    text-decoration: none;
    display: block;
`


const ResultItem = ({ data }: any) => {

    return (
        <StyledListItem>
            <StyledLink to={`/detail/${data.id}`}>
                <Img src={`https://image.tmdb.org/t/p/w500/${data.poster_path}`} />
            </StyledLink>
            <StyledInfoBox>
                <StyledLink to={`/detail/${data.id}`}>
                    <H3>{data.title}<span>{` (${parseInt(data.release_date, 10)})`}</span></H3>

                </StyledLink>
                <P>{data.overview.length <= 120 ? data.overview : data.overview.substring(0, 120) + '...'}</P>
            </StyledInfoBox>
        </StyledListItem>
    )
}

export default ResultItem
