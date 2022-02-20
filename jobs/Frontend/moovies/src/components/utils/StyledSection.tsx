import styled from 'styled-components'

interface SectionProps {
    bgrCol: string
}

const StyledSection = styled.section`
    min-height: 350px;
    width: 100vw;
    background-color: ${(props: SectionProps) => props.bgrCol}
`

export default StyledSection

