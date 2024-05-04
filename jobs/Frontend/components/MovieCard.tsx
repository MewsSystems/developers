"use client";

import { useEffect, useState } from "react";
import Image from "next/image";
import styled from "styled-components";
import { getYear } from "@/utils/date.util";
import Link from "next/link";
import { getImageUrl } from "@/utils/image.util";

const Container = styled.div`
  border-radius: 10px;
  margin: 5px 0;
  width: 300px;
  height: 180px;
  display: flex;
  gap: 5px;
  background: ${(props) => props.theme.card.background};
  border: 1px solid ${(props) => props.theme.card.border};

  &:hover {
    border: 1px solid ${(props) => props.theme.card.borderHover};
  }
`;

const ImagePlaceholder = styled.div`
  width: 120px;
  height: 180px;
  display: flex;
  justify-content: center;
  align-items: center;
`;

const StyledImage = styled(Image)`
  object-fit: cover;
  border-radius: 8px 0 0 8px;
`;

const StyledLink = styled(Link)`
  color: ${(props) => props.theme.primary.text};
  text-decoration: none;
`;

const TextContainer = styled.div`
  padding: 5px;
  display: flex;
  flex: 1;
  flex-direction: column;
  justify-content: center;
`;

const Title = styled.p`
  display: -webkit-box;
  -webkit-line-clamp: 3;
  -webkit-box-orient: vertical;
  overflow: hidden;
  width: 150px;
`;

interface MovieCardProps {
  id: number;
  posterUrl: string | null;
  releaseDate: string;
  title: string;
}

const MovieCard = ({ id, posterUrl, releaseDate, title }: MovieCardProps) => {
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    setLoading(true);
  }, [posterUrl, id, releaseDate, title]);

  const base64Image =
    "data:image/jpeg;base64,iVBORw0KGgoAAAANSUhEUgAAAfQAAABLCAYAAACGPXWeAAABBElEQVR42u3VQREAAAQAMJLL5qGXHM5WYtk9FQDAaSl0ABA6ACB0AEDoAIDQAUDoAIDQAQChAwBCBwChAwBCBwCEDgAIHQCEDgAIHQAQOgAgdAAQOgAgdABA6ACA0AFA6ACA0AEAoQMAQgcAoQMAQgcAhA4ACB0AhA4ACB0AEDoAIHQAEDoAIHQAQOgAgNABQOgAgNABAKEDAEIHAKELHQCEDgAIHQAQOgAgdAAQOgAgdABA6ACA0AFA6ACA0AEAoQMAQgcAoQMAQgcAhA4ACB0AhA4ACB0AEDoAIHQAEDoAIHQAQOgAgNABQOgAgNABAKEDAEIHAKEDAEIHAIQOAAgdAJ5Zv3PQTkffvswAAAAASUVORK5CYII=";

  return (
    <StyledLink href={`movie/${id.toString()}`}>
      <Container>
        {posterUrl ? (
          <StyledImage
            src={loading ? base64Image : getImageUrl(200, posterUrl)}
            alt={title}
            width={120}
            height={180}
            priority
            placeholder="blur"
            blurDataURL={base64Image}
            onLoad={() => setLoading(false)}
          />
        ) : (
          <ImagePlaceholder>No Image</ImagePlaceholder>
        )}
        <TextContainer>
          <Title>{title}</Title>
          <p>{releaseDate ? getYear(releaseDate) : "N/A"}</p>
        </TextContainer>
      </Container>
    </StyledLink>
  );
};

export default MovieCard;
