import { FaStar } from 'react-icons/fa';
import { StyledVote, StyledVoteNumber } from './Vote.styles';
import { parseVoteAverage } from '../../utils';
import type { VoteParams } from '../types';

export const Vote = ({ voteAverage, voteTotalCount }: VoteParams) => {
  const { vote } = parseVoteAverage(voteAverage);

  return (
    <StyledVote>
      <StyledVoteNumber>
        <FaStar className="mr-4" />
        {vote}
      </StyledVoteNumber>
      {`(${voteTotalCount})`}
    </StyledVote>
  );
};
