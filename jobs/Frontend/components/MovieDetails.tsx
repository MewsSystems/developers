"use client";

import { desktopMediaQuery, tabletMediaQuery } from "@/breakpoints";
import { MovieGenre, ProductionCompany } from "@/interfaces/movie";
import { getYear } from "@/utils/date.util";
import { getImageUrl } from "@/utils/image.util";
import Image from "next/image";
import styled from "styled-components";

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
`;

const OverviewContainer = styled.div`
  max-width: 750px;
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
  width: 500px;
  height: 500px;
  display: flex;
  justify-content: center;
  align-items: center;
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
  margin: 0;
  padding: 0;
`;

const TagContainer = styled.div`
  display: flex;
  gap: 10px;
`;

const SmallHeading = styled.h4`
  margin-bottom: 0;
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
        <Title>{title}</Title>
        <TextUnderTitle>
          {getYear(releaseDate)} - {runtime}m
          {genres.length > 0 && (
            <> - {genres?.map((genre) => genre.name).join(", ")}</>
          )}
        </TextUnderTitle>
        {tagline && <p>{tagline}</p>}
        <OverviewContainer>{overview}</OverviewContainer>
        {productionCompanies.length > 0 && (
          <>
            <SmallHeading>Production Companies:</SmallHeading>
            <ul>
              {productionCompanies?.map((productionCompany, index) => (
                <li key={index}>
                  Name: {productionCompany.name}
                  {productionCompany.originCountry && (
                    <> - Origin Country: {productionCompany.originCountry}</>
                  )}
                </li>
              ))}
            </ul>
          </>
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
