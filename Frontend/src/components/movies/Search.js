import React from 'react';

const Search = ({ change }) => {

  return (
    <div>
      <input
        id="search"
        type="search"
        placeholder="Search movies..."
        onChange={e => change(e.target.value)}/>
    </div>
  )
};

export default Search;



