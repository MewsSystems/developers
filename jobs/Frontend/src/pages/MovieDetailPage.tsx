import React from "react";
import { useParams, Link } from "react-router-dom";
import styled from "styled-components";
import Button from "../components/Button";
import PageContainer from "../components/PageContainer";
import { useGetMovieDetail } from "../hooks/movies";
import LoadingSpinner from "../components/LoadingSpinner";

const ResponsiveTwoColumns = styled.div`
  display: flex;
  flex-wrap: wrap;
  gap: 16px;

  max-width: 768px;
  margin: 0 auto;

  @media (max-width: 768px) {
    flex-direction: column;
  }
`;

const Column = styled.div`
  flex: 1;
`;

const RoundedImg = styled.img`
  border-radius: 8px;
  width: 300px;
`;

const MovieDetailPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();

  const { data: movieDetail, isLoading, isError } = useGetMovieDetail(id || "");

  return (
    <PageContainer>
      <Link to="/">
        <Button>Go back to the search page</Button>
      </Link>
      {isLoading && <LoadingSpinner />}
      {isError && <div>Error: Impossible to fetch the data</div>}
      {movieDetail && (
        <ResponsiveTwoColumns>
          <Column>
            <h1>{movieDetail.title}</h1>
            <p data-test="overview">{movieDetail.overview}</p>
          </Column>
          <Column>
            <RoundedImg
              src={`https://image.tmdb.org/t/p/w500${movieDetail.poster_path}`}
              alt={movieDetail.title}
            />
          </Column>
        </ResponsiveTwoColumns>
      )}
    </PageContainer>
  );
};

export default MovieDetailPage;
