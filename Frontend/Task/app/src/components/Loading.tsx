import React from "react";

const Loading: React.FC<{ message: string }> = ({ message }) => {
  return (
    <div>
      {message}
      Loading
    </div>
  );
};

export default Loading;
