import { useEffect, useState } from "react";
import styled from "styled-components";
import { Button, Chip, Rating, StreamingProviders, Typography, WithMovieIdProps } from ".";
import { tmdbClient } from "@/tmdbClient";
import { MovieDetails } from "tmdb-ts";
import { MaxWidthWrapper } from "@/pages/Details"; /* TODO: replace import */
import posterFallback from "@/assets/mocks/poster-fallback.jpg";
import { MEDIA_ORIGINAL_BASE_URL, MEDIA_300_BASE_URL } from "@/tmdbClient";

const OverviewContainer = styled.div<{ $bgPath?: string }>`
  display: grid;
  place-items: center;
  min-height: 65vh;

  &:before {
    content: " ";
    position: absolute;
    z-index: -2;

    width: 100%;
    height: 100%;

    filter: brightness(50%);
    background: ${({ $bgPath }) =>
      $bgPath ? `url(${MEDIA_ORIGINAL_BASE_URL + $bgPath})` : "grey"};
    background-size: cover;
  }

  &:after {
    content: " ";
    position: absolute;
    z-index: -1;

    width: 100%;
    height: 100%;

    backdrop-filter: blur(10px);
  }
`;

const ContentContainer = styled.div`
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 24px;
`;

const Poster = styled.img`
  max-height: 396px;
  border-radius: 28px;
`;

const TextContent = styled.div`
  display: flex;
  flex-direction: column;
  gap: 8px;
`;

// TODO: make it reusable
const ChipsContainer = styled.div`
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
`;

const BottomContent = styled.div`
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 8px;

  margin-top: 16px;
`;

const BottomRightWrapper = styled.div`
  display: flex;
  align-items: center;
  gap: 16px;
`;

export function MovieOverview({ movieId }: WithMovieIdProps) {
  const [data, setData] = useState<MovieDetails>();

  useEffect(() => {
    tmdbClient.movies.details(movieId).then(res => setData(res));
  }, [movieId]);

  return (
    <OverviewContainer $bgPath={data?.backdrop_path}>
      <MaxWidthWrapper>
        <ContentContainer>
          <Poster
            src={data?.poster_path ? MEDIA_300_BASE_URL + data?.poster_path : posterFallback}
          />
          <TextContent>
            <Typography variant="displayMedium" color="white" bold>
              {data?.title}
            </Typography>
            <ChipsContainer>
              {data?.genres.map(genre => (
                <Chip key={genre.id} label={genre.name} TypographyProps={{ color: "white" }} />
              ))}
            </ChipsContainer>
            <Typography color="white">Release date: {data?.release_date}</Typography>
            <Typography variant="titleLarge" color="white">
              Overview:
            </Typography>
            <Typography variant="bodyLarge" color="white">
              {data?.overview}
            </Typography>
            <BottomContent>
              <Rating value={data ? data.vote_average / 2 : 0} />
              <BottomRightWrapper>
                <StreamingProviders movieId={movieId} />
                {data?.homepage && <Button>Go to movie's homepage</Button>}
                {/* TODO: fix button's text color, do as link */}
              </BottomRightWrapper>
            </BottomContent>
          </TextContent>
        </ContentContainer>
      </MaxWidthWrapper>
    </OverviewContainer>
  );
}
