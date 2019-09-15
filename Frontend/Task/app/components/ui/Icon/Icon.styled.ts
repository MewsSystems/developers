import styled from 'styled-components'

export const IconNode = styled.i`
    position: relative;
    display: inline-block;
    min-width: 1em;

    &:after{
        content: '\\00a0';
    }

    svg {
        display: block;
        position: absolute;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        fill: currentColor;
    }
`