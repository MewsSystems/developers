import {colors, spacing} from "../styles/designTokens.ts";
import {keyframes, styled} from "styled-components";

export const Loading = () => {
    return (
        <Container>
            <InnerSvg
                viewBox="0 0 35 35"
                height="35px"
                width="35px"
            >
                <Track
                    x={2.5}
                    y={2.5}
                    fill="none"
                    strokeWidth="2px"
                    width="25px"
                    height="25px"
                />
                <Car
                    x={2.5}
                    y={2.5}
                    fill="none"
                    strokeWidth="5px"
                    width="25px"
                    height="25px"
                    pathLength="100"
                />
            </InnerSvg>
        </Container>
    )
}

export const LoadingWrapper = styled.div`
    padding: ${spacing["2xl"]} ${spacing.lg};

    height: 100vh;
    
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
`;

const Container = styled.div`
    flex-shrink: 0;
    display: inline-flex;
    align-items: center;
    justify-content: center;
    height: 50px;
    width: 50px;
`

const InnerSvg = styled.svg`
    height: 50px;
    width: 50px;
    transform-origin: center;
    will-change: transform;
    overflow: visible;
`

const travel = keyframes`
    from {
        stroke-dashoffset: 0;
    }
    to {
        stroke-dashoffset: -100;
    }
`

const Car = styled.rect`
    fill: none;
    stroke: ${colors.primary};
    stroke-dasharray: 25, 75;
    stroke-dashoffset: 0;
    animation: ${travel} 2s linear infinite;
    will-change: stroke-dasharray, stroke-dashoffset;
    transition: stroke 0.5s ease;
`

const Track = styled.rect`
    fill: none;
    stroke: ${colors.primary};
    opacity: 0;
    transition: stroke 0.5s ease;
`