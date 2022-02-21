import styled from 'styled-components'
import ImgPlaceholder from './utils/ImgPlaceholder'
import { CastObject } from './SearchResultsList'
import { H3 } from './utils/Typography'

const WrapDiv = styled.div`
    box-sizing: content-box;
    display: flex;
    flex-direction: column;
    align-items: center;
    min-width: 130px;
    max-width: 130px;
    padding: 1rem;

    && img {
        height: 180px;
        max-width: 120px;
        object-fit: cover;
    }
`

const NameSpan = styled(H3)`
    font-weight: 700;
    font-size: 0.85rem;
    text-align: center;
    max-width: 140px;
    margin-top: 0.5rem;
`

const CharSpan = styled.span`
    font-weight: 300;
    font-size: 0.85rem;
    text-align: center;
    max-width: 140px;
    line-height: 1.2;
`

interface Props {
    data: CastObject
}

const CastItem = (props: Props) => {

    const { name, character, profile_path } = props.data

    return (
        <WrapDiv>
            {profile_path
                ?
                <img src={`https://image.tmdb.org/t/p/w500/${profile_path}`} />
                :
                <ImgPlaceholder width="120px" height="180px">
                    <span>no image available</span>
                </ImgPlaceholder>}
            <NameSpan>{name}</NameSpan>
            <CharSpan>{character}</CharSpan>
        </WrapDiv>
    )
}

export default CastItem
