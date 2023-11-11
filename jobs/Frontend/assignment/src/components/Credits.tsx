import { useEffect, useState } from "react";
import styled from "styled-components";
import { Avatar, Typography, WithMovieIdProps } from ".";
import { tmdbClient } from "@/tmdbClient";
import { Credits } from "tmdb-ts";

const CreditsWrapper = styled.div`
  display: flex;
  gap: 16px;
  width: 100%;

  overflow-x: auto;
`;

export function Credits({ movieId }: WithMovieIdProps) {
  const [data, setData] = useState<Credits>();

  useEffect(() => {
    tmdbClient.movies.credits(movieId).then(res => setData(res));
  }, [movieId]);

  return (
    <CreditsWrapper>
      {data?.cast.length || data?.crew.length ? (
        <>
          {data?.cast.map(({ name, profile_path, character, credit_id }) => (
            <Avatar key={credit_id} imgPath={profile_path} name={name} description={character} />
          ))}
          {data?.crew.map(({ name, profile_path, job, credit_id }) => (
            <Avatar key={credit_id} imgPath={profile_path} name={name} description={job} />
          ))}
        </>
      ) : (
        <Typography variant="titleMedium" color="secondary">
          No credits found
        </Typography>
      )}
    </CreditsWrapper>
  );
}
