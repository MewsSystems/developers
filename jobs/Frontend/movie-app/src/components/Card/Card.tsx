import { Link } from "react-router-dom";
import { CardImage, CardWrapper } from "./Card.styles";

type CardProps = {
  cardDetailLink: string;
  cardImage: string;
  altText: string;
  observerRef?: React.LegacyRef<HTMLDivElement>;
};

function Card({ cardDetailLink, cardImage, altText, observerRef }: CardProps) {
  return (
    <Link to={cardDetailLink}>
      <CardWrapper ref={observerRef}>
        <CardImage src={cardImage} alt={altText} />
      </CardWrapper>
    </Link>
  );
}
export default Card;
