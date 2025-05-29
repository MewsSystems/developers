import styled, {keyframes} from 'styled-components';

const LoadingOverlay = styled.div`
  position: fixed;
  inset: 0;
  background-color: white;
  display: flex;
  justify-content: center;
  align-items: center;
`;

const spin = keyframes`
  from { transform: rotate(0deg); }
  to { transform: rotate(360deg); }
`;

const Spinner = styled.div`
  width: 48px;
  height: 48px;
  border: 4px solid #e5e7eb;
  border-top-color: #3b82f6;
  border-radius: 50%;
  animation: ${spin} 1s linear infinite;
`;

export function LoadingSpinner() {
  return (
    <LoadingOverlay>
      <Spinner />
    </LoadingOverlay>
  );
} 