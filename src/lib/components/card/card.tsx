import { ReactNode } from 'react';
import { UserScore } from '../user-score/user-score';
import { CardImageContainer, ScoreWrapper, CardBody, CardTitle, CardFooter, CardContainer } from './card.styled';
import { Img } from '../image/image';

export interface CardProps {
  children: React.ReactNode;
  onClick?: () => void;
}

const CardImage = ({ src, alt, score }: any) => {
  return (
    <CardImageContainer>
      <Img src={src} alt={alt} />
      {score ? (
        <ScoreWrapper size={36}>
          <UserScore percent={score} />
        </ScoreWrapper>
      ) : null}
    </CardImageContainer>
  );
};

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