import React from "react";
import "./spinner.styles.scss";

export const Spinner = () => {
  return (
    <div className="bg">
      <div className="fancy-spinner">
        <div className="ring"></div>
        <div className="ring"></div>
        <div className="dot">
          <div className="cross"></div>
        </div>
      </div>
      <div className="waitPlease">Please wait, your data is loading....</div>
    </div>
  );
};
