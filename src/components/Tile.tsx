// Tile.tsx
import "../CSS/Tile.css";

const Tile: React.FC<TileProps> = (tileProps: TileProps) => {
  const {
    item,
    openDetail,
  } = tileProps;
  const releaseYear = new Date(item.release_date).getFullYear();

  return (
    <div className="tile-container">
      {/* Image display, backdrop > poster > null */}
      <div className="img-shadow" onClick={() => openDetail(tileProps)}>
        {item.backdrop_path != null ? (
          <img
            src={"https://image.tmdb.org/t/p/original/" + item.backdrop_path}
          />
        ) : item.poster_path != null ? (
          <img
            src={"https://image.tmdb.org/t/p/original/" + item.poster_path}
          />
        ) : (
          <div className="img-placeholder" />
        )}
      </div>

      {/* Movie description */}
      <div className="description-container">
        <h2>{item.original_title}</h2>
        <div className="description-text">
          <div className="description-release">
            <b>Released:</b>
            <p className="release">
              {!Number.isNaN(releaseYear) ? releaseYear : "-"}
            </p>
          </div>
        </div>
        <p className="vote-avg">{item.vote_average.toPrecision(2)}/10</p>
      </div>
    </div>
  );
};

export default Tile;
