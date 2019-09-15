
import styled from 'styled-components'

export const LoaderWord = styled.div`
    display: inline-block;
    border-radius: 2px;
    min-height: 1em;
    max-width: 100%;

    &.LoaderWord--WithSpaceAfter{
        margin-right: 1em;
    }

    &.LoaderBackground {
        background: -webkit-gradient(linear, left top, right top, from(rgba(207, 216, 220, 0.2)), color-stop(rgba(207, 216, 220, 0.4)), to(rgba(207, 216, 220, 0.2)));
        background: -webkit-linear-gradient(left, rgba(207, 216, 220, 0.2), rgba(207, 216, 220, 0.4), rgba(207, 216, 220, 0.2));
        background: linear-gradient(90deg, rgba(207, 216, 220, 0.2), rgba(207, 216, 220, 0.4), rgba(207, 216, 220, 0.2));
        -webkit-animation: loading-background 2.5s ease infinite;
        animation: loading-background 2.5s ease infinite;
        background-size: 600% 600%;
    }

    @-webkit-keyframes loading-background {
        0%,
        100% {
            background-position: 0 50%;
        }
        50% {
            background-position: 100% 50%;
        }
    }
    @keyframes loading-background {
        0%,
        100% {
            background-position: 0 50%;
        }
        50% {
            background-position: 100% 50%;
        }
    }
`