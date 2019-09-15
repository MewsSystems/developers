import styled from 'styled-components'

interface CenterContainerProps {
    maxWidth?: string,
}

export const CenterContainer = styled.div<CenterContainerProps>`
    max-width: ${(props: CenterContainerProps) => props.maxWidth};
    margin: 0 auto;
    padding: 0 15px;
`

CenterContainer.defaultProps = {
    maxWidth: "1080px"
}