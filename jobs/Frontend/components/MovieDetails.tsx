"use client";

import { tabletMediaQuery } from "@/breakpoints";
import { MovieGenre, ProductionCompany } from "@/interfaces/movie";
import { getImageUrl } from "@/utils/image.util";
import Image from "next/image";
import { Fragment } from "react";
import styled from "styled-components";

const Container = styled.div`
  display: flex;
  gap: 50px;
  flex-direction: column;

  ${tabletMediaQuery} {
    flex-direction: row;
  }
`;

const TextContainer = styled.div`
  border: 1px solid ${(props) => props.theme.primary.border};
  border-radius: 5px;
  padding: 10px;
  display: flex;
  flex-direction: column;
`;

const OverviewContainer = styled.div`
  max-width: 750px;
`;

const StyledImage = styled(Image)`
  border-radius: 10px;
  width: 250px;
  height: 400px;
  align-self: center;

  ${tabletMediaQuery} {
    width: 500px;
    height: 750px;
  }
`;

const ImagePlaceholder = styled.div`
  width: 500px;
  height: 500px;
  display: flex;
  justify-content: center;
  align-items: center;
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
          blurDataURL="data:image/jpeg;base64,iVBORw0KGgoAAAANSUhEUgAAAfQAAABLCAYAAACGPXWeAAABBElEQVR42u3VQREAAAQAMJLL5qGXHM5WYtk9FQDAaSl0ABA6ACB0AEDoAIDQAUDoAIDQAQChAwBCBwChAwBCBwCEDgAIHQCEDgAIHQAQOgAgdAAQOgAgdABA6ACA0AFA6ACA0AEAoQMAQgcAoQMAQgcAhA4ACB0AhA4ACB0AEDoAIHQAEDoAIHQAQOgAgNABQOgAgNABAKEDAEIHAKELHQCEDgAIHQAQOgAgdAAQOgAgdABA6ACA0AFA6ACA0AEAoQMAQgcAoQMAQgcAhA4ACB0AhA4ACB0AEDoAIHQAEDoAIHQAQOgAgNABQOgAgNABAKEDAEIHAKEDAEIHAIQOAAgdAJ5Zv3PQTkffvswAAAAASUVORK5CYII="
        />
      ) : (
        <ImagePlaceholder>No Image</ImagePlaceholder>
      )}
      <TextContainer>
        <h1>{title}</h1>
        {tagline && <>{tagline}</>}
        <OverviewContainer>{overview}</OverviewContainer>
        <p>Release Date: {releaseDate}</p>
        <p>Genres: {genres?.map((genre) => genre.name).join(", ")}</p>
        <p>Original Language: {originalLanguage.toUpperCase()}</p>
        <p>Original Title: {originalTitle}</p>
        Production Companies:
        {productionCompanies?.map(
          (
            productionCompany, // TODO - seperator
            index,
          ) => (
            <Fragment key={index}>
              Name: {productionCompany.name}
              {productionCompany.originCountry && (
                <>Origin Country: {productionCompany.originCountry}</>
              )}
            </Fragment>
          ),
        )}
        <p>Runtime: {runtime}</p>
        <p>Status: {status}</p>
      </TextContainer>
    </Container>
  );
};

export default MovieDetails;
