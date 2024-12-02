"use client";

import Image from "next/image";
import Link from "next/link";
import styled from "styled-components";
import { getYear } from "@/utils/date.util";
import { getImageUrl, IMAGE_PLACEHOLDER_URL } from "@/utils/image.util";
import { useSearchParams } from "next/navigation";

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

// A skeleton loading state would be nice on these cards
const MovieCard = ({ id, posterUrl, releaseDate, title }: MovieCardProps) => {
  const searchParams = useSearchParams();
  const query = searchParams.get("query") ?? "";
  const currentPage = Number(searchParams.get("page")) || 1;

  return (
    <StyledLink
      href={`movie/${id.toString()}?query=${query}&page=${currentPage}`}
      prefetch={false}
    >
      <Container>
        {posterUrl ? (
          <StyledImage
            src={getImageUrl(200, posterUrl)}
            alt={title}
            width={120}
            height={180}
            priority
            placeholder="blur"
            blurDataURL={IMAGE_PLACEHOLDER_URL}
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
