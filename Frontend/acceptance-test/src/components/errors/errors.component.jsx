import React from "react";
import "./errors.styles.scss";

const Errors = ({ errors }) => {
  return (
    <>{errors ? <p className="container__title errors">{errors}</p> : null}</>
  );
};

export default Errors;
