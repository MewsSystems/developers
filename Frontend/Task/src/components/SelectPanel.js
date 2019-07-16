import React, { useState } from "react";
import { connect } from "react-redux";
import {
  saveTrendPairs,
  saveConfigurationData,
  saveFilterConfiguration
} from "../store/actions/storageActions";
import styles from "./SelectPanel.css";

const SelectPanel = props => {
  const handleChange = event => {
    const tempOptions = [...event.target.selectedOptions].map(opt => opt.value);
    props.saveConfigurationData(tempOptions);
  };

  return (
    <div className={styles.dash__controls}>
      {props.selectConfiguration && props.selectConfiguration.length > 2 ? (
        <select
          className={styles.sort__select}
          multiple={true}
          size="10"
          value={props.allCurrencies}
          onChange={e => handleChange(e)}
        >
          {props.selectConfiguration &&
            props.selectConfiguration.map((item, index) => (
              <option
                className={styles.sort__option}
                key={item}
                value={item[0]}
              >
                {item[1][0][1]}-/-{item[1][1][1]}
                ->
                {item[2][0][1]}-/-{item[2][1][1]}
              </option>
            ))}
        </select>
      ) : (
        <div className={styles.empty__box}>
          <h3 className={styles.info__text}>Loading data from server</h3>
        </div>
      )}
    </div>
  );
};

const mapStateToProps = state => ({
  trendPairs: state.trendPairs,
  allCurrencies: state.allCurrencies,
  selectConfiguration: state.selectConfiguration
});

const mapDispatchToProps = {
  saveTrendPairs,
  saveConfigurationData,
  saveFilterConfiguration
};

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(SelectPanel);
