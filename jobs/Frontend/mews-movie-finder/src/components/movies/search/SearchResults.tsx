import { Movie } from "../../../types/movies";
import { HStack, VStack } from "../../shared/Stacks";
import { PaginatedResponse } from "../../../types/requests";
import { MovieDetail } from "../detail/MovieDetail";
import { ShadowBox } from "../../shared/Boxes";
import { MovieList } from "./MovieList";
import { useState } from "react";
import { H3 } from "../../shared/Headings";

interface ISearchResults {
  page: PaginatedResponse<Movie> | undefined;
  isLoading: boolean;
  search: string;
  setPage: React.Dispatch<React.SetStateAction<number>>;
}

export function SearchResults(props: ISearchResults) {
  const [selected, setSelected] = useState(0);

  const getSearchInfo = () => {
    if (props.search == "") {
      return "A list of movies will display here...";
    }
    if (props.isLoading) {
      return `Loading movies for "${props.search}"...`;
    }
    if (props.search.length < 4) {
      return "Keep typing...";
    }
    if (props.page != undefined && props.page.results.length > 0) {
      return `Found these movies for "${props.search}"`;
    }
    if (props.page != undefined && props.page.results.length == 0) {
      return `No movies found for "${props.search}"`;
    }
    
  };

  return (
    <HStack>
      <ShadowBox $width="50%" $marginRight="24px">
        <VStack $width="100%">
          <H3 $fontFamily="Exo" $fontSize="24px" data-testid="search-info">{getSearchInfo()}</H3>
          {props.page != undefined && (
            <MovieList page={props.page} setPage={props.setPage} setSelected={setSelected}/>
          )}
        </VStack>
      </ShadowBox>
      <MovieDetail selectedMovieId={selected}/>
    </HStack>
  );
}
