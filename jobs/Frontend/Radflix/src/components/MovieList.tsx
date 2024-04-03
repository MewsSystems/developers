// MovieList.tsx
import { useState } from "react";

import Tile from "./Tile";
import MovieDetail from "./MovieDetail";

import "../CSS/MovieList.css";

interface MovieListProps {
  results: [] | undefined;
  score: string;
  visible: boolean;
}

const MovieList: React.FC<MovieListProps> = ({ results, visible }) => {
  const [detail, setDetail] = useState(false);
  const [movie, setMovie] = useState<SearchResult>();

  let sortedResults: SearchResult[] = [];

  // Sort results by popularity
  const sortResults = (results: []) => {
    return [...results].sort(
      (a: SearchResult, b: SearchResult) =>
        parseFloat(b.popularity!) - parseFloat(a.popularity!)
    );
  };

  // Pass movie details + toggle modal
  const openDetail = (movie: TileProps) => {
    setMovie(movie.item);
    setDetail(true);
  };

  sortedResults = sortResults(results!);

  return (
    <div
      className="list-container"
      style={visible ? { opacity: 100 } : { opacity: 0 }}
    >
      {sortedResults ? (
        <div className="list-fade">
          {sortedResults.map((item: SearchResult, i) => (
            <Tile key={i} item={item} openDetail={openDetail} />
          ))}
        </div>
      ) : null}
      {movie ? (
        <MovieDetail
          movie={movie}
          isOpen={detail}
          closeDetail={() => setDetail(false)}
        />
      ) : null}
    </div>
  );
};

export default MovieList;
