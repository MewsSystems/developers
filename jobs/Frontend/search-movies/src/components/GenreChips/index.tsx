import { useEffect, useState } from "react";
import styled from "styled-components";
import { genre } from "../../utils/movieGenre";
import Chip from "../Chip";

const ChipListContainer = styled.div`
  display: flex;
  justify-content: left;
  flex-wrap: wrap;
`;

interface GenreChipsProps {
  genreIds: number[];
}

const GenreChips = (props: GenreChipsProps) => {
  const { genreIds } = props;
  const [genreNames, setGenreNames] = useState<string[]>([]);
  console.log("this i s genre ", genreIds);

  useEffect(() => {
    (function () {
      const ids: string[] = [];
      Array.isArray(genreIds) &&
        genreIds.forEach((id) => {
          if (typeof id === "number") {
            const genreName = genre.find((g) => g.id === id)?.name;
            if (genreName) {
              ids.push(genreName);
            }
          }
          if (typeof id === "object") {
            const idObj = id as any;
            ids.push(idObj.name);
          }
        });
      setGenreNames(ids);
    })();
  }, []);

  return (
    <ChipListContainer>
      {genreNames.map((name) => (
        <Chip label={name} />
      ))}
    </ChipListContainer>
  );
};

export default GenreChips;
