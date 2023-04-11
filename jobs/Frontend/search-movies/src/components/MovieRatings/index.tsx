import { FaStar } from "react-icons/fa";
import styled from "styled-components";
import { colors } from "../../utils/theme";
import Chip from "../Chip";

const MovieRatingsContainer = styled.div`
  color: ${colors.primaryText};
  display: flex;
`;

const StarContainer = styled.span`
  margin: 18px 15px 0 0;
  svg {
    color: yellow;
  }
`;

const RatingInfo = styled.p`
  margin-right: 15px;
`;

interface MovieRatingsProps {
  rating: number;
  ratingCount: number;
}

/**
 * Rating for a movie
 * @param props {rating, ratingCount} rating for a movie 
 * @returns renders rating for a movie
 */
const MovieRatings = (props: MovieRatingsProps) => {
  const { rating, ratingCount } = props;
  return (
    <MovieRatingsContainer>
      <StarContainer>
        <FaStar />
      </StarContainer>
      <RatingInfo>{rating} / 10</RatingInfo>
      <Chip heading="Voted By" label={ratingCount} />
    </MovieRatingsContainer>
  );
};

export default MovieRatings;
