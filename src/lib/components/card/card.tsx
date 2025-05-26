import { ReactNode } from 'react';
import { CardBody, CardContainer, CardFooter, CardTitle } from './card.styled';
import { CardImage } from './card-image';


export interface CardProps {
  children: React.ReactNode;
  onClick?: () => void;
}

export const Card = ({ children, onClick }: CardProps): ReactNode => {
  return (
    <CardContainer onClick={onClick}>
      {children}
    </CardContainer>
  );
};

Card.Image = CardImage;
Card.Body = CardBody;
Card.Title = CardTitle;
Card.Footer = CardFooter;