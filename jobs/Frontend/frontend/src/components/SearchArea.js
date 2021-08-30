import React from 'react';
import './style.css';

const SearchArea = (props) => {
  return (
    <div>
      <div>
        <section>
          <form
            onSubmit={(e) => {
              e.preventDefault();
            }}
          >
            <div>
              <input
                placeholder="Search movie"
                type="text"
                onChange={props.handleChange}
              />
            </div>
          </form>
        </section>
      </div>
    </div>
  );
};

export default SearchArea;
