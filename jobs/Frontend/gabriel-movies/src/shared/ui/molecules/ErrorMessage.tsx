import styled from "styled-components";
import { Subtitle } from "../atoms/Typography/Subtitle";
import { Text } from "../atoms/Typography/Text";

const Wrap = styled.div`
  border: 1px solid ${({ theme }) => theme.colors.danger};
  background: ${({ theme }) => theme.colors.bgDark};
  padding: 0.75rem 1rem;
  border-radius: 8px;
`;

export function ErrorMessage({ message }: { message: string }) {
  return (
    <Wrap role="alert">
      <Subtitle>Something went wrong</Subtitle>
      <Text>{message}</Text>
    </Wrap>
  );
}
