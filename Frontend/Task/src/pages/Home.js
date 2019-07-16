import React from "react";
import SelectPanel from "../components/SelectPanel";
import CurrencyDisplay from "../components/CurrencyDisplay";

import styles from "./Home.css";

const Home = props => {
  return (
    <div className={styles.app__container}>
      <SelectPanel />
      <CurrencyDisplay />
    </div>
  );
};

export default Home;
