
import { UserScore } from '../user-score/user-score';
import { Img } from '../image/image';
import { CardImageContainer, ScoreWrapper } from './card.styled';

interface CardImageProps {  
    src: string;
    alt: string;
    score?: number;
    loading?: 'eager' | 'lazy';
  }
export const CardImage = ({ src, alt, score, loading = 'lazy' }: CardImageProps) => {
    return (
      <CardImageContainer>
        <Img src={src} alt={alt} loading={loading} />
        {score ? (
          <ScoreWrapper size={36}>
            <UserScore percent={score} />
          </ScoreWrapper>
        ) : null}
      </CardImageContainer>
    );
};