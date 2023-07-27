import { FC } from "react";
import Styled from "styled-components";
import Card from "../card/card";
import { CardProps } from "../card/card.types";
import { CardListProps } from "./cards-list.types";

// Create a Wrapper component that'll render a <section> tag with some styles

const CardsContainer = Styled.section`
  display: flex;
  flex-wrap: wrap;
justify-content: start;
gap: 16px 30px;
`;

const CardsList: FC<CardListProps> = ({ numberOfCards }) => {
 


  const auxArray = Array.from({ length: numberOfCards }, (_, i) => i + 1);
  const cardsList = auxArray.map((number) => {
    const mockCardData:CardProps  = {
        id:number,
        isAdultFilm: true,
        collections: "fantastics",
        orginalLanguage: "ES",
        originalTitle: "The title og",
        title: "the movie title",
        posterPath: "https://m.media-amazon.com/images/M/MV5BYmQ2M2I3YTQtOGU4MC00YWNjLThhNjYtMWJmY2RjYzlkMDZkXkEyXkFqcGdeQXVyMTA0MDM3NDg4._V1_FMjpg_UX682_.jpg",
        releaseDate: new Date(),
      }

    return(<Card key={number} movie={mockCardData}></Card>)
});

  return <CardsContainer>{cardsList}</CardsContainer>;
};

export default CardsList;
