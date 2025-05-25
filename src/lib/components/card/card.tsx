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
  height: 300px;
  width: 180px;
  display: flex;
  flex-direction: column;
`;

const Image = styled.img`
  width: 100%;
  height: 200px;
`;

const CardImageContainer = styled.div`
  width: 100%;
  height: 200px;
`;

const CardBody = styled.div`
    padding: 10px;
    position: relative;
    white-space: normal;
    display: flex;
    flex-wrap: wrap;
    gap: 2px;
    flex: 1;
`;

const CardTitle = styled.h3`
  margin: 0;
  font-size: 1rem;
  color: #333;
  font-weight: 700
`;

const CardFooter = styled.div`
  padding: 10px;
  color: #666;
  font-size: 0.875rem;
  margin-top: auto;
`;

const CardImage = ({ src, alt }: any) => {
  return (
    <CardImageContainer>
      <Image src={src} alt={alt} />
    </CardImageContainer>
  );
};

export const Card: React.FC<CardProps> & {
  Image: typeof CardImage;
  Body: typeof CardBody;
  Title: typeof CardTitle;
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
Card.Footer = CardFooter;