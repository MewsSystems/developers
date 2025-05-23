import React from 'react';
import styled from 'styled-components';

export interface CardProps {
  children: React.ReactNode;
  onClick?: () => void;
  style?: React.CSSProperties;
}

const CardContainer = styled.div`
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  overflow: hidden;
  transition: transform 0.2s ease-in-out;

  &:hover {
    transform: translateY(-4px);
  }
`;

const CardImage = styled.img`
  width: 100%;
  height: 400px;
  object-fit: cover;
`;

const CardBody = styled.div`
  padding: 1rem;
`;

const CardTitle = styled.h3`
  margin: 0;
  font-size: 1.25rem;
  color: #333;
`;

const CardDescription = styled.p`
  margin: 0.5rem 0;
  color: #666;
  font-size: 0.875rem;
  line-height: 1.5;
  display: -webkit-box;
  -webkit-line-clamp: 3;
  -webkit-box-orient: vertical;
  overflow: hidden;
`;

const CardFooter = styled.div`
  padding: 1rem;
  border-top: 1px solid #eee;
  color: #666;
  font-size: 0.875rem;
`;

export const Card: React.FC<CardProps> & {
  Image: typeof CardImage;
  Body: typeof CardBody;
  Title: typeof CardTitle;
  Description: typeof CardDescription;
  Footer: typeof CardFooter;
} = ({ children, onClick, style }) => {
  return (
    <CardContainer onClick={onClick} style={style}>
      {children}
    </CardContainer>
  );
};

Card.Image = CardImage;
Card.Body = CardBody;
Card.Title = CardTitle;
Card.Description = CardDescription;
Card.Footer = CardFooter;
