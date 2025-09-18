import styled from "styled-components";
import { Subtitle } from "../atoms/Typography/Subtitle";
import { Text } from "../atoms/Typography/Text";

const Wrap = styled.div`
  text-align: center;
`;

export function EmptyState({ title, subtitle }: { title: string; subtitle?: string }) {
  return (
    <Wrap role="status">
      <Subtitle>{title}</Subtitle>
      {subtitle && <Text>{subtitle}</Text>}
    </Wrap>
  );
}
