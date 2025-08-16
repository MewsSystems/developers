import styled, { css } from 'styled-components';
import { LoadingBarProps } from './LoadingBar';

const StyledLoadingBar = styled.div<LoadingBarProps>`
    background: repeating-linear-gradient(
        to right,
        red 0%,
        ${(props) => props.theme.colors.primary} 50%,
        red 100%
    );
    width: 100%;
    height: 4px;
    background-size: 200% auto;
    box-shadow: 2px 4px 10px rgba(0, 0, 0, 0.15);
    border-radius: 20px;
    background-position: 0 100%;
    position: absolute;
    bottom: 0;
    left: 0;
    animation: gradient 1.5s infinite;
    animation-fill-mode: forwards;
    animation-timing-function: linear;

    ${(props) =>
        !props.loading &&
        css`
            animation: none;
            background: ${props.theme.colors.primary};
        `}

    @keyframes gradient {
        0% {
            background-position: 0 0;
        }
        100% {
            background-position: -200% 0;
        }
    }
`;

export default StyledLoadingBar;
