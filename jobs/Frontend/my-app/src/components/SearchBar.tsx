// import { useState } from 'react';

export const SearchBar = () => {
  //   const [query, setQuery] = useState('');
  return (
    <>
      <label>
        <input
          type="text"
          placeholder="Enter movie name"
          //   onChange={(e) => setQuery(e.target.value)}
        />
      </label>
    </>
  );
};
