interface MovieGenresProps {
  genres: Array<string>;
}

const MovieGenres = ({ genres }: MovieGenresProps) => {
  return (
    <>
      {genres.map((genre) => (
        <span
          key={genre}
          className="bg-gray-100 text-sm me-1 mb-1 px-2 py-1 rounded-md"
        >
          {genre}
        </span>
      ))}
    </>
  );
};

export default MovieGenres;
