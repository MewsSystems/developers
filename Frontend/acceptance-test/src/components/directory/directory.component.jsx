import React from "react";

//Components
import Card from "../card/card.component";
import Errors from "../errors/errors.component";

//Styles
import "./directory.styles.scss";

const Directory = ({ movies, errors }) => {
  return (
    <main className="container">
      <h1 className="container__title">Movies</h1>
      {errors ? <Errors errors={errors} /> : null}
      {movies.results.length !== 0 ? (
        <Card movies={movies.results} />
      ) : (
        <span className="nodata">No data, sorry for not being styled yet</span>
      )}
    </main>
  );
};

export default Directory;
