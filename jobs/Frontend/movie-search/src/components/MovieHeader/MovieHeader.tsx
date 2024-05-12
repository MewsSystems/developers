"use client";
import styled from "styled-components";
import MovieSearchInput from "../MovieSearchInput/MovieSearchInput";

const HeaderContainer = styled.header`
  background-color: #2c3e50;
  padding: 20px 0;
  color: #ffffff;
`;

const Title = styled.h1`
  text-align: center;
  margin: 0;
  padding: 0 20px;
  font-size: 24px;
`;

export default function MovieHeader() {
  return (
    <HeaderContainer>
      <Title>MEWS Movie Search</Title>
      <MovieSearchInput />
    </HeaderContainer>
  );
}
