import { styled } from "styled-components";
import { Card } from "../../../../components/Card";

const Container = styled.ul`
  padding: 0;
  margin: 16px 0;
  list-style: none;
`;

const Item = styled(Card)`
  &:not(:last-child) {
    margin-bottom: 8px;
  }

  p {
    margin: 0;
  }

  & > a {
    width: 100%;
    height: 100%;

    padding: 16px;

    text-decoration: none;
    color: black;

    display: flex;
    align-items: center;
    object-fit: cover;

    gap: 16px;
  }
`;

const ItemImage = styled.img`
  height: 128px;
  aspect-ratio: 2/3;
  object-fit: cover;
  border-radius: 4px;
`;

const LoaderWrapper = styled.div`
  display: flex;
  align-items: center;
  justify-content: center;
`;

const MovieTitle = styled.span`
  font-size: 1.25rem;
  font-weight: 700;
`;

const MovieYear = styled.span`
  font-size: 0.875rem;
`;

export const MovieListStyle = {
  Container,
  Item,
  LoaderWrapper,
  ItemImage,
  MovieTitle,
  MovieYear,
};
