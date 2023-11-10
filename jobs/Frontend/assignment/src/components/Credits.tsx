import { useEffect, useState } from "react";
import styled from "styled-components";
import { Avatar, WithMovieIdProps } from ".";
import { tmdbClient } from "@/pages/Search";
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
  }, []);

  return (
    <CreditsWrapper>
      {data?.cast.map(({ name, profile_path, character }) => (
        <Avatar imgPath={profile_path} name={name} description={character} />
      ))}
      {data?.crew.map(({ name, profile_path, job }) => (
        <Avatar imgPath={profile_path} name={name} description={job} />
      ))}
    </CreditsWrapper>
  );
}
