import { FC } from "react";
import Styled from "styled-components";
import { useAppSelector } from "../../store/store";
import Card from "../card/card";

// Create a Wrapper component that'll render a <section> tag with some styles

const CardsContainer = Styled.section`
  display: flex;
  flex-wrap: wrap;
justify-content: start;
gap: 16px 30px;
`;

const CardsList: FC<{}> = () => {
  const movies = useAppSelector((state) => state.movies.foundMoviesPage);

  const cardsList = movies?.movies.map((value, index) => {
    return <Card key={index} movie={value}></Card>;
  });

  return <CardsContainer>{cardsList}</CardsContainer>;
};

export default CardsList;
