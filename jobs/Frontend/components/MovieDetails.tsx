"use client";

import { tabletMediaQuery } from "@/breakpoints";
import { MovieGenre, ProductionCompany } from "@/interfaces/movie";
import { getYear } from "@/utils/date.util";
import { getImageUrl } from "@/utils/image.util";
import Image from "next/image";
import styled from "styled-components";

const Container = styled.div`
  display: flex;
  gap: 50px;
  flex-direction: column;
  justify-content: center;
  margin-top: 40px;

  ${tabletMediaQuery} {
    flex-direction: row;
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

const TextUnderTitleContainer = styled.div`
  display: flex;
  gap: 5px;
`;

const TextUnderTitle = styled.p`
  margin-top: 0;
  padding-top: 0;
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
          blurDataURL="data:image/jpeg;base64,iVBORw0KGgoAAAANSUhEUgAAAfQAAABLCAYAAACGPXWeAAABBElEQVR42u3VQREAAAQAMJLL5qGXHM5WYtk9FQDAaSl0ABA6ACB0AEDoAIDQAUDoAIDQAQChAwBCBwChAwBCBwCEDgAIHQCEDgAIHQAQOgAgdAAQOgAgdABA6ACA0AFA6ACA0AEAoQMAQgcAoQMAQgcAhA4ACB0AhA4ACB0AEDoAIHQAEDoAIHQAQOgAgNABQOgAgNABAKEDAEIHAKELHQCEDgAIHQAQOgAgdAAQOgAgdABA6ACA0AFA6ACA0AEAoQMAQgcAoQMAQgcAhA4ACB0AhA4ACB0AEDoAIHQAEDoAIHQAQOgAgNABQOgAgNABAKEDAEIHAKEDAEIHAIQOAAgdAJ5Zv3PQTkffvswAAAAASUVORK5CYII="
        />
      ) : (
        <ImagePlaceholder>No Image</ImagePlaceholder>
      )}
      <TextContainer>
        <Title>{title}</Title>
        <TextUnderTitleContainer>
          <TextUnderTitle>{getYear(releaseDate)}</TextUnderTitle>-
          <TextUnderTitle>{runtime}m</TextUnderTitle>-
          <TextUnderTitle>
            {genres?.map((genre) => genre.name).join(", ")}
          </TextUnderTitle>
        </TextUnderTitleContainer>
        {tagline && <>{tagline}</>}
        <OverviewContainer>{overview}</OverviewContainer>
        <h4>Production Companies:</h4>
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
