import styled from 'styled-components'

export const MovieElementWrapper = styled.li`
    margin-right: 8px;
    min-width: 220px;
    figcaption {
        font-size: 16px;
        font-weight: 700;
        min-height: 80px;
        span {
            font-size: 14px;
            font-weight: 500;
        }
    }
    img {
        &:hover {
            opacity: 0.8;
        }
    }
`
