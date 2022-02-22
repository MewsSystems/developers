import styled from 'styled-components'
import { Link } from 'react-router-dom'
import { H3 } from './utils/Typography'
import ImgPlaceholder from './utils/ImgPlaceholder'

const StyledListItem = styled.div`
    display: flex;
    background-color: #f3f3f3;
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
    width: 100px;
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
    max-height: 150px;
`


const ResultItem = ({ data }: any) => {

    return (
        <StyledListItem>
            <StyledLink to={`/detail/${data.id}`}>
                {data.poster_path ? <Img src={`https://image.tmdb.org/t/p/w500/${data.poster_path}`} /> : <ImgPlaceholder width="100px" height="150px"><span>no image available</span></ImgPlaceholder>}
            </StyledLink>
            <StyledInfoBox>
                <StyledLink to={`/detail/${data.id}`}>
                    <H3>{data.title}<span>{` (${parseInt(data.release_date, 10)})`}</span></H3>
                </StyledLink>
                {data.overview
                    ?
                    <P>{data.overview.length <= 125 ? data.overview : data.overview.substring(0, 125) + '...'}</P>
                    :
                    <P>no description available</P>
                }
            </StyledInfoBox>
        </StyledListItem>
    )
}

export default ResultItem
