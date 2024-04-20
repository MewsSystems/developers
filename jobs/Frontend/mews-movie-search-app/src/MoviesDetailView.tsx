/* Global imports */
import * as React from "react";
import styled from "styled-components";
import { Link, useParams } from "wouter";
import { useMovieDetail } from "./useMovieDetail";
import { imageDomainURL } from "./utils/constant";
import { LoadingMessage } from "./Loading";

/* Local imports */

/* Types  */
/* Local utility functions */
const separateIntoCommas = (strings: string[]) => {
  const strWithCommas = strings.map((str) => str.concat(",")).join(" ");
  return strWithCommas.trim().lastIndexOf(",") === strWithCommas.length - 1
    ? strWithCommas.slice(0, -1)
    : strWithCommas;
};
/* Component definition */
export const MoviesDetailView = () => {
  const params = useParams<{ id: string }>();
  const { movieDetail, isLoading } = useMovieDetail(Number(params.id));

  return (
    <MovieDetailLayout>
      <Column>
        <Row>
          <Column>
            <Link to={"/"}> Back</Link>
          </Column>
        </Row>
        <Gap />
        {isLoading ? (
          <LoadingMessage />
        ) : movieDetail ? (
          <Row>
            <Poster>
              <img
                style={{ borderRadius: "10px" }}
                src={`${imageDomainURL}w400/${movieDetail?.poster_path}`}
                alt={movieDetail?.title}
              />
            </Poster>
            <Content>
              <Wrapper>
                <Title>{movieDetail?.title}</Title>
                <Text size="lg">🎖️{movieDetail?.popularity}</Text>
              </Wrapper>
              <Wrapper>
                <Text>
                  🗓️{" "}
                  {new Date(
                    movieDetail?.release_date ?? ""
                  ).toLocaleDateString()}
                  -💰{" "}
                  {new Intl.NumberFormat("en", {
                    style: "currency",
                    currency: "USD",
                  }).format(movieDetail?.budget)}
                </Text>
              </Wrapper>
              <Wrapper>
                <Text>{movieDetail.overview}</Text>
              </Wrapper>
              <Separator />
              <Wrapper>
                <Text size="md">
                  Genre:
                  {separateIntoCommas(
                    movieDetail.genres.map((gen) => gen.name)
                  )}
                </Text>
              </Wrapper>
            </Content>
          </Row>
        ) : (
          <span>No data found</span>
        )}
      </Column>
    </MovieDetailLayout>
  );
};
const Gap = styled.div`
  padding: 1rem;
`;
const Row = styled.div`
  display: flex;
  flex-direction: row;
`;
const Column = styled.div`
  display: flex;
  flex-direction: column;
`;
const Separator = styled.hr`
  border: 0px;
  height: 1px;
  background: rgba(0, 0, 0, 0.5);
  width: 100%;
`;
const MovieDetailLayout = styled.div`
  display: flex;
  padding: 1rem;
  height: 100vh;
`;

const Poster = styled.section`
  display: flex;
  justify-content: center;
`;
const Content = styled.section`
  padding: 1rem;
  width: 100%;
  display: flex;
  flex: 1;
  flex-direction: column;
`;

const Wrapper = styled.div`
  margin: 1rem 0px;
  display: flex;
  justify-content: space-between;
  align-items: center;
`;

const Title = styled.h3`
  font-size: 2rem;
  font-weight: bolder;
`;
const Text = styled.span<{ size?: "base" | "xs" | "sm" | "md" | "lg" | "xl" }>`
  font-size: ${(props) => {
    switch (props.size) {
      case "xs":
        return "0.25rem";
      case "sm":
        return "0.5rem";
      case "md":
        return "0.75rem";
      case "lg":
        return "1.5rem";
      case "xl":
        return "3rem";
      default:
        return "1rem";
    }
  }};
`;
