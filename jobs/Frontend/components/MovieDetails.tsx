"use client";

import Image from "next/image";
import styled from "styled-components";
import { desktopMediaQuery } from "@/breakpoints";
import { MovieGenre, ProductionCompany } from "@/interfaces/movie";
import { getYear } from "@/utils/date.util";
import { getImageUrl, IMAGE_PLACEHOLDER_URL } from "@/utils/image.util";
import ProductionCompanies from "./ProductionCompanies";

const Container = styled.div`
  display: flex;
  gap: 10px;
  flex-direction: column;
  justify-content: center;
  margin-top: 10px;

  ${desktopMediaQuery} {
    flex-direction: row;
    padding: 0 20px;
    margin-top: 40px;
    margin-bottom: 40px;
    gap: 50px;
  }
`;

const TextContainer = styled.div`
  border: 1px solid ${(props) => props.theme.card.border};
  background: ${(props) => props.theme.card.background};
  border-radius: 5px;
  padding: 20px 40px;
  display: flex;
  flex-direction: column;

  ${desktopMediaQuery} {
    max-width: 750px;
  }
`;

const StyledImage = styled(Image)`
  border-radius: 10px;
  width: 250px;
  height: 400px;
  align-self: center;

  ${desktopMediaQuery} {
    width: 500px;
    height: 750px;
  }
`;

const ImagePlaceholder = styled.div`
  width: 100%;
  height: 400px;
  display: flex;
  justify-content: center;
  align-items: center;

  ${desktopMediaQuery} {
    width: 500px;
    height: 750px;
  }
`;

const Tag = styled.div`
  background: ${(props) => props.theme.tag.background};
  width: 100px;
  height: 25px;
  display: flex;
  justify-content: center;
  align-items: center;
  // Could have different coloured tags for different statuses
  color: ${(props) => props.theme.secondary.text};
  border-radius: 8px;
`;

const Title = styled.h1`
  margin-bottom: 0;
  padding-bottom: 0;
`;

const TextUnderTitle = styled.p`
  margin: 0 0 15px 0;
  padding: 0;
`;

const TagContainer = styled.div`
  display: flex;
  gap: 10px;
`;

interface MovieDetailsProps {
  genres: MovieGenre[];
  originalLanguage: string;
  originalTitle: string;
  overview: string;
  productionCompanies: ProductionCompany[];
  posterUrl: string | null;
  releaseDate: string;
  runtime: number;
  status: string;
  tagline: string;
  title: string;
}

const MovieDetails = ({
  genres,
  originalLanguage,
  originalTitle,
  overview,
  posterUrl,
  productionCompanies,
  releaseDate,
  runtime,
  status,
  tagline,
  title,
}: MovieDetailsProps) => {
  return (
    <Container>
      {posterUrl ? (
        <StyledImage
          src={getImageUrl(500, posterUrl)}
          alt={title}
          width={500}
          height={750}
          priority
          placeholder="blur"
          blurDataURL={IMAGE_PLACEHOLDER_URL}
          // Currently the NextJS server is optimising and caching these images, in the real world you'd want to serve them from a CDN and have some form of image optimisation/transformation
        />
      ) : (
        <ImagePlaceholder>No Image</ImagePlaceholder>
      )}
      <TextContainer>
        <Title>{title}</Title>
        <TextUnderTitle>
          {getYear(releaseDate)} - {runtime}m
          {genres.length > 0 && (
            <> - {genres?.map((genre) => genre.name).join(", ")}</>
          )}
        </TextUnderTitle>
        {tagline && <p>{tagline}</p>}
        <p>{overview}</p>
        {productionCompanies.length > 0 && (
          <ProductionCompanies productionCompanies={productionCompanies} />
        )}
        <p>Original Title: {originalTitle}</p>
        <TagContainer>
          <Tag>{status}</Tag>
          <Tag>Lang: {originalLanguage.toUpperCase()}</Tag>
        </TagContainer>
      </TextContainer>
    </Container>
  );
};

export default MovieDetails;
