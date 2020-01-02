import React from "react";
import "./styles.module.css";

const RateName: React.FC<{}> = ({ currency }) => {
  return (
    <>
      <span>{`${currency[0].name} / ${currency[1].name}`}</span>
      <span>{`${currency[0].code} / ${currency[1].code}`}</span>
    </>
  );
};

export default RateName;
