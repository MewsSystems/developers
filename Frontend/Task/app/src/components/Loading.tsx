import React from "react";

const Loading: React.FC<{ message: string }> = ({ message }) => {
  return (
    <div className="center">
      <div>{message}</div>
      <div className="lds-circle">
        <div></div>
      </div>
    </div>
  );
};

export default Loading;
