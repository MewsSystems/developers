import './searchForm.css';

export default function SearchForm({
  searchValue,
  searchFunction,
}: {
  searchValue: any;
  searchFunction: any;
}) {
  return (
    <form className="search_form">
      <label htmlFor="movie">Search for the movie: </label>
      <input
        type="text"
        id="movie"
        value={searchValue}
        onChange={searchFunction}
        placeholder="type the movie name..."
        className="movie_serach_input"
      />
      <button className="clear_button">x</button>
    </form>
  );
}
