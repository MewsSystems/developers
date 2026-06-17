import styled from "styled-components";
import { Chip, LinkButton, Rating, Typography } from "..";
import fallbackImg from "@/assets/mocks/fallback.jpg";
import { Movie } from "tmdb-ts";
import { MEDIA_300_BASE_URL } from "@/tmdbClient";

export interface MovieCardProps {
  movie: Movie;
  genres?: string[];
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

export function MovieCard({ movie, genres }: MovieCardProps) {
  const {
    id,
    title,
    overview,
    release_date: releaseDate,
    vote_average,
    poster_path: posterPath,
  } = movie;

  return (
    <CardWrapper>
      <CardImage
        src={posterPath ? MEDIA_300_BASE_URL + posterPath : fallbackImg}
        alt="Movie poster"
      />
      <CardContent>
        <TextWrapper>
          <Typography variant="titleSmall">{title}</Typography>
          <Typography variant="bodySmall" color="secondary">
            Release date: {releaseDate || "Unknown"}
          </Typography>
          <DescriptionWrapper>
            <Typography variant="bodySmall">{overview}</Typography>
          </DescriptionWrapper>
          <ChipsWrapper>
            {genres?.map((genre, index) => <Chip key={genre + index} label={genre} />)}
          </ChipsWrapper>
        </TextWrapper>
        <ActionsWrapper>
          <Rating value={vote_average / 2} />
          <LinkButton href={`/movie/${id}`}>See more</LinkButton>
        </ActionsWrapper>
      </CardContent>
    </CardWrapper>
  );
}
