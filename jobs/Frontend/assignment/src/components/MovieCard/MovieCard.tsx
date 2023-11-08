import styled from "styled-components";
import { Button, Chip, Rating, Typography } from "..";
import BrokenImageIcon from "@material-ui/icons/BrokenImage";
import { useNavigate } from "react-router-dom";

export interface MovieCardProps {
  id: number;
  title: string;
  description: string;
  releaseDate: string;
  genres: string[];
  rating: number;
  imgPath: string | null;
}

const CardWrapper = styled.div`
  display: flex;
  flex-direction: column;

  max-height: 540px;
  width: 100%;

  border-radius: 16px;
  overflow: hidden;
  border: 1px solid lightgrey;
`;

const CardImageWrapper = styled.div`
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  gap: 16px;

  height: 310px;

  color: ${({ theme }) => theme.colors.primary.main};
`;

const CardImage = styled.img`
  height: 310px;
  object-fit: cover;
`;

const CardContent = styled.div`
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  gap: 16px;

  flex-grow: 1;

  padding: 16px;
`;

const TextWrapper = styled.div`
  display: flex;
  flex-direction: column;
  gap: 8px;

  height: 100%;
`;

const StyledDescription = styled(Typography)`
  display: -webkit-box;
  -webkit-line-clamp: 3;
  -webkit-box-orient: vertical;
  overflow: hidden;
`;

const ChipsWrapper = styled.div`
  display: flex;
  gap: 8px;
`;

const ActionsWrapper = styled.div`
  display: flex;
  gap: 8px;
  justify-content: space-between;
  align-items: center;
`;

export function MovieCard({
  id,
  title,
  description,
  releaseDate,
  genres,
  rating,
  imgPath,
}: MovieCardProps) {
  const navigate = useNavigate();
  const navigateToDetails = () => {
    navigate(`/movie/${id}`);
  };

  return (
    <CardWrapper>
      {imgPath ? (
        <CardImage src={imgPath} alt="Movie poster" />
      ) : (
        <CardImageWrapper>
          <BrokenImageIcon fontSize="large" />
          <Typography variant="titleSmall">Image loading failed</Typography>
        </CardImageWrapper>
      )}
      <CardContent>
        <TextWrapper>
          <div>
            <Typography variant="titleSmall">{title}</Typography>
            <Typography variant="bodySmall">Release date: {releaseDate}</Typography>
          </div>
          <StyledDescription variant="bodySmall">{description}</StyledDescription>
          <ChipsWrapper>
            {genres.map(genre => (
              <Chip key={genre} label={genre} />
            ))}
          </ChipsWrapper>
        </TextWrapper>
        <ActionsWrapper>
          <Rating value={rating} />
          <Button onClick={navigateToDetails}>See more</Button>
        </ActionsWrapper>
      </CardContent>
    </CardWrapper>
  );
}
