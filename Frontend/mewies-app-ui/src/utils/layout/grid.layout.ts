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
    flex: ${props => props.size};
    padding: 0 5px 0 5px;
`
