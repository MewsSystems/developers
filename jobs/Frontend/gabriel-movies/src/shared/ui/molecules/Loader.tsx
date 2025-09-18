import styled, { keyframes } from "styled-components";
import { Text } from "../atoms/Typography/Text";

const spin = keyframes`to { transform: rotate(360deg); }`;

const Wrap = styled.div`
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
`;

const Spinner = styled.span`
  width: 24px; 
  height: 24px;
  border: 2px solid ${({ theme }) => theme.colors.link};
  border-top-color: transparent;
  border-radius: 50%;
  display: inline-block;
  animation: ${spin} 0.8s linear infinite;
`;

export function Loader({ label = "Loading..." }: { label?: string }) {
  return (
    <Wrap role="status">
      <Spinner aria-hidden="true" />
      <Text>{label}</Text>
    </Wrap>
  );
}
