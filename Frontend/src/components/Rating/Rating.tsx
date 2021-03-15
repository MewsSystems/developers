import { Star } from '@styled-icons/fa-solid';
import Flex from '../common/Flex';
import FlexIconText from '../common/FlexIconText';

interface RatingProps {
  voteAverage: number;
  voteCount: number;
  voteMax?: number;
}

function Rating({ voteAverage, voteCount, voteMax = 10 }: RatingProps) {
  return (
    <FlexIconText icon={Star}>
      <Flex flexDirection="column">
        <span>
          <strong className="vote-average">{voteAverage}</strong>
          <small>/{voteMax}</small>
        </span>
        <small className="vote-count">{voteCount} votes</small>
      </Flex>
    </FlexIconText>
  );
}

export default Rating;
