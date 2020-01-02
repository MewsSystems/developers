import React from "react";
import "./styles.module.css";

const RateName = ({ currency }) => {
  return (
    <>
      <span className="name">{`${currency[0].name} / ${currency[1].name}`}</span>
    </>
  );
};

export default RateName;
