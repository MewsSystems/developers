import { ReactNode } from 'react';
import { CardBody, CardContainer, CardFooter, CardTitle } from './card.styled';
import { CardImage } from './card-image/card-image';

export interface CardProps {
  children: React.ReactNode;
  onClick?: () => void;
  ariaLabel?: string;
}

export const Card = ({ children, onClick, ariaLabel }: CardProps): ReactNode => {
  return <CardContainer aria-label={`View details of ${ariaLabel}`} onClick={onClick}>{children}</CardContainer>;
};

Card.Image = CardImage;
Card.Body = CardBody;
Card.Title = CardTitle;
Card.Footer = CardFooter;
