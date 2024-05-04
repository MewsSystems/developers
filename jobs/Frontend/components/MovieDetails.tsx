"use client";

import { MovieGenre, ProductionCompany } from "@/interfaces/movie";
import { getImageUrl } from "@/utils/image.util";
import Image from "next/image";
import styled from "styled-components";

const Container = styled.div`
  display: flex;
  gap: 50px;
`;

const TextContainer = styled.div`
  display: flex;
  flex-direction: column;
`;

const StyledImage = styled(Image)`
  border-radius: 10px;
`;

const ImagePlaceholder = styled.div`
  width: 500px;
  height: 500px;
  display: flex;
  justify-content: center;
  align-items: center;
`;

interface MovieDetailsProps {
  budget: number;
  genres: MovieGenre[];
  originalLanguage: string;
  originalTitle: string;
  overview: string;
  productionCompanies: ProductionCompany[];
  posterUrl: string | null;
  releaseDate: string;
  revenue: number;
  runtime: number;
  status: string;
  tagline: string;
  title: string;
}

const MovieDetails = ({
  budget,
  genres,
  originalLanguage,
  originalTitle,
  overview,
  posterUrl,
  productionCompanies,
  releaseDate,
  revenue,
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
        <p>{title}</p>
        <p>{tagline}</p>
        <p>{releaseDate}</p>
        <p>{budget}</p>
        <p>{genres?.map((genre) => genre.name)}</p>
        <p>{originalLanguage}</p>
        <p>{originalTitle}</p>
        <p>{overview}</p>
        <p>
          {productionCompanies?.map((productionCompany) => [
            productionCompany.name,
            productionCompany.originCountry,
          ])}
        </p>
        <p>{revenue}</p>
        <p>{runtime}</p>
        <p>{status}</p>
      </TextContainer>
    </Container>
  );
};

export default MovieDetails;
