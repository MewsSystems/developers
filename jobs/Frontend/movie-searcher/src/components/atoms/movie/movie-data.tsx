import React from "react";
import styled from "styled-components";

export const MovieData: React.FC<{
  title: string;
  content: string | React.ReactNode;
  link?: string;
}> = ({ title, content, link }) => {
  return (
    <Container>
      <Title>{title}</Title>
      {link ? (
        <StyledLink href={link} target="_blank" rel="noopener noreferrer">
          {content}
        </StyledLink>
      ) : (
        <Content>{content}</Content>
      )}
    </Container>
  );
};

const Container = styled.div`
  margin-top: 1.5rem;
  margin-right: 1.5rem;
`;

const Title = styled.h2`
  font-size: 1.125rem;
  font-weight: 600;
`;

const StyledLink = styled.a`
  color: #b3b3b3;
  text-decoration: none;

  &:hover {
    color: white;
  }
`;

const Content = styled.p`
  color: #b3b3b3;
`;
