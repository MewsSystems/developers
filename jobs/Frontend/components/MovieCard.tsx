"use client";

import imageLoader from "@/loader";
import Image from "next/image";
import styled from "styled-components";
import { getYear } from "@/utils/date.util";
import Link from "next/link";

const Container = styled.div`
  border: 1px solid ${(props) => props.theme.primary.border};
  border-radius: 10px;
  margin: 5px 0;
  width: 250px;
  display: flex;
  gap: 5px;
`;

const ImagePlaceholder = styled.div`
  width: 120px;
  height: 200px;
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
`;

interface MovieCardProps {
  id: number;
  posterUrl: string | null;
  releaseDate: string;
  title: string;
}

const MovieCard = ({ id, posterUrl, releaseDate, title }: MovieCardProps) => {
  return (
    <StyledLink href={`movie/${id.toString()}`}>
      <Container>
        {posterUrl ? (
          <StyledImage
            src={posterUrl}
            alt={title}
            loader={imageLoader}
            width={120}
            height={200}
            priority
            // I have not optimised the quality or generated a wider srcset of these images for this task, as the MovieDb does not support it
          />
        ) : (
          <ImagePlaceholder>No Image</ImagePlaceholder>
        )}
        <TextContainer>
          <p>{title}</p>
          <p>{getYear(releaseDate)}</p>
        </TextContainer>
      </Container>
    </StyledLink>
  );
};

export default MovieCard;
