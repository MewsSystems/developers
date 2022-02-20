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
    width: 100px;
`

const ImgPlaceholder = styled.div`
    background-color: #f8f8f8;
    width: 100px;
    height: 150px;
    border: 3px solid #d8d8d8;
    color: #c8c8c8;
    font-size: 0.8rem;
    display: flex;
    justify-content: center;
    align-items: center;
    text-align: center;
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
                {data.poster_path ? <Img src={`https://image.tmdb.org/t/p/w500/${data.poster_path}`} /> : <ImgPlaceholder><span>no image available</span></ImgPlaceholder>}
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
