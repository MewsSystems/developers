import React, { useState } from "react";
import { withRouter } from "react-router-dom";

//Font awesome
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faSearch } from "@fortawesome/free-solid-svg-icons";

//Styles
import "./search-bar.styles.scss";

const SearchBar = ({ placeholder, history }) => {
  const [movieSearch, setMovieSearch] = useState('');

  const searchHandleChange = e => {
    setMovieSearch(e.target.value);
  };

  const submitForm = eventSearch => {
    eventSearch.preventDefault();
    history.push(`/search/${movieSearch}`);
  };

  return (
    <form action="#" className="search" onSubmit={submitForm}>
      <input
        type="text"
        className="search__input"
        placeholder={placeholder}
        onChange={searchHandleChange}
      />
      <button className="search__button">
        <FontAwesomeIcon
          icon={faSearch}
          className="search__icon"
          color="#55c57a"
        />
      </button>
    </form>
  );
};

export default withRouter(SearchBar);
