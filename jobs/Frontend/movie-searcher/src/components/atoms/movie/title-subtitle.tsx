import React from "react";
import styled from "styled-components";

export const TitleAndSubtitle: React.FC<{
  title: string;
  subtitle: string;
}> = ({ title, subtitle }) => {
  return (
    <Container>
      <Title>{title}</Title>
      <Subtitle>{subtitle}</Subtitle>
    </Container>
  );
};

const Container = styled.div`
  display: flex;
  flex-direction: column;
`;

const Title = styled.h1`
  font-size: 2.5rem;
  font-weight: bold;
  color: #ffffff;
`;

const Subtitle = styled.p`
  font-size: 0.875rem;
  color: #9ca3af;
  font-style: italic;
  margin-top: 0.25rem;
`;
