import React from 'react';
import {
  StyledCard,
  CardHeader,
  CardBody,
  CardFooter,
  CardImage,
  CardImageContainer,
  CardTitle,
  CardDescription
} from './card-styled';

interface CardProps {
  children: React.ReactNode;
  variant?: 'default' | 'movie' | 'profile';
}

interface CardComposition {
  Header: React.FC<{ children: React.ReactNode }>;
  Body: React.FC<{ children: React.ReactNode }>;
  Footer: React.FC<{ children: React.ReactNode }>;
  Image: React.FC<{ src: string; alt: string }>;
  Title: React.FC<{ children: React.ReactNode }>;
  Description: React.FC<{ children: React.ReactNode }>;
}

const Card: React.FC<CardProps> & CardComposition = ({ children, variant = 'default' }) => {
  return (
    <StyledCard variant={variant}>
      {children}
    </StyledCard>
  );
};

Card.Header = ({ children }) => <CardHeader>{children}</CardHeader>;
Card.Body = ({ children }) => <CardBody>{children}</CardBody>;
Card.Footer = ({ children }) => <CardFooter>{children}</CardFooter>;
Card.Image = ({ src, alt }) => (
  <CardImageContainer>
    <CardImage src={src} alt={alt} />
  </CardImageContainer>
);
Card.Title = ({ children }) => <CardTitle>{children}</CardTitle>;
Card.Description = ({ children }) => <CardDescription>{children}</CardDescription>;

export { Card };
