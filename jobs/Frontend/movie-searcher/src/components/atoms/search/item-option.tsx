import React from "react";
import { useNavigate } from "react-router-dom";
import styled, { css } from "styled-components";
import { Movie } from "../../../models/tmdbModels";

export const ItemOption: React.FC<{
  params: React.HTMLAttributes<HTMLLIElement> & { key: any };
  size: "small" | "large";
  option: Movie;
}> = ({ params, option, size }) => {
  const nav = useNavigate();

  return (
    <StyledListItem
      {...params}
      key={option.id + option.title}
      onClick={() => nav(`movie/${option.id}`)}
    >
      <Container>
        <PosterContainer>
          <Poster
            src={`https://image.tmdb.org/t/p/w500/${option.poster_path}`}
            alt={option.title}
          />
        </PosterContainer>
        <Title size={size}>{option.title}</Title>
      </Container>
    </StyledListItem>
  );
};

const StyledListItem = styled.li`
  list-style: none;
  cursor: pointer;
  transition: background-color 0.3s ease-in-out;

  &:hover {
    background-color: rgba(196, 171, 156, 0.2);
  }
`;

const Container = styled.div`
  display: flex;
  align-items: center;
  border-bottom: 1px solid #e5e7eb;
  padding: 0.5rem;
`;

const PosterContainer = styled.div`
  display: flex;
  width: 20%;
  align-items: center;
`;

const Poster = styled.img`
  width: 2.5rem;
  height: 2.5rem;
`;

const Title = styled.div<{ size: "small" | "large" }>`
  margin-left: 1.25rem;
  width: 80%;
  font-weight: bold;

  ${({ size }) =>
    size === "small"
      ? css`
          font-size: 9px;
        `
      : css`
          font-size: 12px;
        `}

  @media (min-width: 1024px) {
    font-size: 14px;
  }
`;
