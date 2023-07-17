import { styled } from "styled-components";
import { Card } from "../Card";

// Shamelessly copied from the internet. :D
export const Spinner = styled.div<{ spinnerColor?: string }>`
  width: 50px;
  height: 50px;
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 4px;

  .spinner-item {
    height: 40%;
    background-color: ${({ spinnerColor = "white" }) => spinnerColor};
    width: calc(50px / 13);
    animation: spinner5 1000ms ease-in-out infinite;
    border-radius: 0;

    @keyframes spinner5 {
      25% {
        transform: scaleY(2);
      }

      50% {
        transform: scaleY(1);
      }
    }
  }

  .spinner-item:nth-child(2) {
    animation-delay: calc(1000ms / 10);
  }

  .spinner-item:nth-child(3) {
    animation-delay: calc(1000ms / 10 * 2);
  }

  .spinner-item:nth-child(4) {
    animation-delay: calc(1000ms / 10 * 3);
  }

  .spinner-item:nth-child(5) {
    animation-delay: calc(1000ms / 10 * 4);
  }
`;

export const Loader = ({ color }: { color?: string }) => (
  <Spinner
    aria-live="polite"
    role="progressbar"
    aria-valuetext="Loading"
    aria-busy="true"
    spinnerColor={color}
  >
    <Card className="spinner-item" />
    <Card className="spinner-item" />
    <Card className="spinner-item" />
    <Card className="spinner-item" />
    <Card className="spinner-item" />
  </Spinner>
);
