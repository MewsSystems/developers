import { useEffect, useState } from "react";
import { Images } from "tmdb-ts";
import styled from "styled-components";
import { Typography, WithMovieIdProps } from ".";
import { tmdbClient } from "@/tmdbClient";
import { MEDIA_500_BASE_URL } from "@/tmdbClient";

const MediaContainer = styled.div`
  display: flex;
  flex-wrap: wrap;
  gap: 24px;

  max-height: 650px;
  overflow-y: auto;
`;

const MediaImage = styled.img`
  border-radius: 28px;
  min-width: 545px;
`;

export function MovieImages({ movieId }: WithMovieIdProps) {
  const [data, setData] = useState<Images>();

  useEffect(() => {
    tmdbClient.movies.images(movieId).then(res => setData(res));
  }, [movieId]);

  return (
    <MediaContainer>
      {data?.backdrops.length ? (
        data?.backdrops.map(({ file_path }) => (
          <MediaImage key={file_path} src={MEDIA_500_BASE_URL + file_path} />
        ))
      ) : (
        <Typography variant="titleMedium" color="secondary">
          No images found
        </Typography>
      )}
    </MediaContainer>
  );
}
