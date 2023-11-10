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
  imgPath: string | null; // TODO: replace with one prop movie
}

const CardWrapper = styled.div`
  display: flex;
  flex-direction: column;

  max-height: 540px;
  min-width: 260px;
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

  flex: 1;
  max-height: 100%;
  overflow: hidden;

  padding: 16px;
`;

const TextWrapper = styled.div`
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  gap: 8px;

  flex: 1;
  max-height: 75%;
  overflow: hidden;

  height: 100%;
`;

const DescriptionWrapper = styled.div`
  display: -webkit-box;
  -webkit-line-clamp: 3;
  -webkit-box-orient: vertical;
  overflow: hidden;
  flex: 1;
  max-height: 35%;
  overflow: hidden;
`;

const ChipsWrapper = styled.div`
  display: flex;
  flex-wrap: wrap;
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
          <Typography variant="titleSmall">{title}</Typography>
          <Typography variant="bodySmall" color="secondary">
            Release date: {releaseDate || "Unknown"}
          </Typography>
          <DescriptionWrapper>
            <Typography variant="bodySmall">{description}</Typography>
          </DescriptionWrapper>
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
