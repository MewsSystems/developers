import { useState } from "react";
import { Input } from "../../components/Form/Input";
import { MainContent } from "../../components/Layout/MainContent";
import { Header } from "../../components/Header/Header";

import { Heading } from "../../components/Typography/Heading";
import { useDebounce } from "../../hooks/useDebounce";
import { MoviesList } from "./MoviesList";

export const HomePage = () => {
  const [searchString, setSearchString] = useState<string>("");

  const debouncedTerm = useDebounce(searchString, 500);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSearchString(e.target.value);
  };

  return (
    <>
      <Header>
        <Input
          name="searchField"
          value={searchString}
          onChange={handleChange}
        />
      </Header>
      <MainContent>
        <Heading>List of movies</Heading>
        <MoviesList debouncedTerm={debouncedTerm} />
      </MainContent>
    </>
  );
};
