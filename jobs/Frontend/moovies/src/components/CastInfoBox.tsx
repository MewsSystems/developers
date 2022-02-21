import styled from 'styled-components'
import CastItem from './CastItem'
import { CastObject } from './SearchResultsList'
import { H2 } from './utils/Typography'

const FlexWrap = styled.div`
    display: flex;
    flex-wrap: wrap;
`

const Wrap = styled.div`
    margin-left: 1.5rem;
`
interface Props {
    cast: CastObject[]
}

const CastInfoBox = (props: Props) => {

    const { cast } = props
    const shortCast = cast.slice(0, 11)
    const castItems = shortCast.map(person => <CastItem data={person} />)

    return (
        <Wrap>
            <H2>Cast ({cast.length})</H2>
            <FlexWrap>
                {castItems}
            </FlexWrap>
        </Wrap>
    )
}

export default CastInfoBox
