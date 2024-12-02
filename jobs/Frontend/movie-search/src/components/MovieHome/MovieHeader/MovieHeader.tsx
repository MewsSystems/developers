"use client";
import MovieSearchInput from "../MovieSearchInput/MovieSearchInput";
import { HeaderContainer, Title } from "./MovieHeaderStyledComponents";

export default function MovieHeader() {
  return (
    <HeaderContainer>
      <Title>MEWS Movie Search</Title>
      <MovieSearchInput />
    </HeaderContainer>
  );
}
