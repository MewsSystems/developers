import styled from 'styled-components'
import { Colors } from '../../utils/constants/color.constants'

export const MovieHeader = styled.header`
    margin: 32px 0 32px 0;
`

export const MovieDetailsSection = styled.section`
    display: flex;
`

export const MovieDetailsContent = styled.article`
    max-width: 520px;
    margin-left: 26px;
    h1 {
        font-size: 42px;
    }
    p:last-of-type,
    h3 {
        margin: 16px 0 16px 0;
    }
`

export const MovieDetailsGenres = styled.ul`
    margin: 16px 0 16px 0;
    display: flex;
    list-style: none;
    li {
        border: 1px solid ${Colors.black};
        padding: 4px;
        font-size: 12px;
        margin-right: 6px;
    }
`

export const MovieListWrapper = styled.ul`
    list-style: none;
    display: flex;
`

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
