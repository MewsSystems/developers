import { HttpError } from "src/api/fmdb";
import { FallbackProps } from "./ErrorBoundary";
import styled from "styled-components";

function ErrorFallback({ error }: FallbackProps) {
  const message =
    error instanceof HttpError && error.status >= 500
      ? "FMDB server is down. Please try later."
      : error.message;
  return (
    <CenteredContainer role="alert">
      <p>Oops!</p>
      <pre>{message}</pre>
    </CenteredContainer>
  );
}

const CenteredContainer = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  height: 100vh;
  p {
    font-size: xxx-large;
    font-weight: 900;
    color: var(--color-light-gray);
  }
`;

export default ErrorFallback;
