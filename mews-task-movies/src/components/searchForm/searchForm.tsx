import './searchForm.css';
import { Ref, forwardRef } from 'react';

export default forwardRef(function SearchForm(
  {
    searchValue,
    searchFunction,
  }: {
    searchValue: any;
    searchFunction: any;
  },
  ref: Ref<HTMLLabelElement>,
) {
  return (
    <form className="search_form">
      <label htmlFor="movie" ref={ref}>
        Search for the movie:{' '}
      </label>
      <input
        type="text"
        id="movie"
        value={searchValue}
        onChange={searchFunction}
        placeholder="type the movie name..."
        className="movie_serach_input"
      />
      <button className="clear_button">❌</button>
    </form>
  );
});
