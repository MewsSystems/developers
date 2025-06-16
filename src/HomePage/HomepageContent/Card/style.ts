import styled from "styled-components";
import { Link } from "react-router";

export const CardLink = styled(Link)`
    aspect-ratio: 2 / 3;
    overflow: hidden;
    border-radius: 4px;
    transition: transform 0.2s ease;

    &:hover {
        transform: scale(1.05);
    }

    &:focus-visible {
        outline: 2px solid white;
        outline-offset: 4px;
    }
`

export const PosterImage = styled.img`
    width: 100%;
    height: 100%;
    border-radius: 4px;
    display: block;
    object-fit: cover;
`