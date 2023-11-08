import styled from "styled-components";
import { Button, Chip, Rating, Typography } from "..";
import { useNavigate } from "react-router-dom";
import fallbackImg from "@/assets/mocks/fallback.jpg";

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
  border: 1px solid ${({ theme }) => theme.colors.outline.variant};
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
      <CardImage src={imgPath || fallbackImg} alt="Movie poster" />
      <CardContent>
        <TextWrapper>
          <div>
            <Typography variant="titleSmall">{title}</Typography>
            <Typography variant="bodySmall" color="secondary">
              Release date: {releaseDate || "Unknown"}
            </Typography>
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
