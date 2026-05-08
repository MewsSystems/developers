import React from "react";
import styled from "styled-components";
import StarIcon from "@mui/icons-material/Star";

export const MoviePunctuation: React.FC<{
  avg: number;
  totalVotes: number;
}> = ({ avg, totalVotes }) => {
  return (
    <Container>
      <PunctuationBadge avg={avg}>
        {avg.toFixed(1)} <StarIcon fontSize="inherit" />
      </PunctuationBadge>
      <Votes>{totalVotes} votes</Votes>
    </Container>
  );
};

const Container = styled.div`
  display: flex;
  align-items: center;
  gap: 0.5rem;
`;

const PunctuationBadge = styled.span<{ avg: number }>`
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0.25rem 0.75rem;
  border-radius: 0.375rem;
  font-size: 0.875rem;
  font-weight: bold;
  color: white;
  background-color: ${({ avg }) =>
    avg >= 8
      ? "#4caf50"
      : avg >= 6
      ? "#edb20e"
      : avg >= 4
      ? "#ff9800"
      : "#f44336"};
`;

const Votes = styled.span`
  font-size: 0.875rem;
  color: #9ca3af;
`;
