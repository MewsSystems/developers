import styled from 'styled-components'

export const Container = styled.div`
    max-width: 1180px;
    margin: 0 auto;
    padding: 0 10px 0 10px;
    @media only screen and (max-width: 960px) {
    }
`

export const Row = styled.div`
    display: flex;
`

export const Col = styled.div<Col>`
    ${props => props.flex && 'display: flex'};
    ${props => props.flex && props.alignCenter && 'align-items: center'};
    ${props => props.flex && props.justifyCenter && 'justify-content: center'};
    ${props =>
        props.flex && props.justifyBetween && 'justify-content: space-between'};
    ${props => props.flex && props.column && 'flex-direction: column'};
    flex: ${props => props.size};
    flex-basis: ${props => (props.size / 12) * 100}%;
`
