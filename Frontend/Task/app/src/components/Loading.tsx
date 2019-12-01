import React from "react";
import styles from "../style.module.css";

const Loading: React.FC<{ message: string }> = ({ message }) => {
  return (
    <div className={styles.center}>
      <div>{message}</div>
      <div className={styles.ldsCircle}>
        <div></div>
      </div>
    </div>
  );
};

export default Loading;
