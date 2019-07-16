import React, { useState, useEffect } from "react";
import axios from "axios";
import usePrevious from "../use-previous";
import { connect } from "react-redux";
import {
  saveTrendPairs,
  saveConfigurationData,
  saveFilterConfiguration
} from "../store/actions/storageActions";
import { CSSTransition, TransitionGroup } from "react-transition-group";
import styles from "./CurrencyDisplay.css";

const CurrencyDisplay = props => {
  const [previousMap, setPreviousMap] = useState([]);
  const [ratesDataArray, setRatesData] = useState([]);
  const [selectedArray, setSelectedArray] = useState([]);
  const [selectedOptions, setSelectedOptions] = useState([]);
  const [errormessage, setErrormessage] = useState("");
  const previousData = usePrevious(selectedArray) || [];

  useEffect(() => {
    let previousMap = new Map();
    previousData.forEach((item, index) => {
      previousMap.set(index, [item[0][0][1], item[1][0][1], item[2][1]]);
    });
    setPreviousMap(previousMap);
  }, [ratesDataArray, props.allCurrencies]);

  useEffect(() => {
    let pickedItems = [];
    for (let currencyItem of props.selectConfiguration) {
      let result = [];
      result = selectedOptions.find(itemId => itemId[0] === currencyItem[0]);
      if (result) {
        pickedItems.push([currencyItem[1], currencyItem[2], result]);
      }
    }
    setSelectedArray(pickedItems);
    let selectedItems = [];
    for (let rate of ratesDataArray) {
      let result = [];
      result = props.allCurrencies.find(itemId => itemId === rate[0]);
      if (result) {
        selectedItems.push([result, rate[1]]);
      }
    }
    setSelectedOptions(selectedItems);
  }, [ratesDataArray, props.allCurrencies]);

  useEffect(() => {
    let newMap = new Map();
    selectedArray.forEach((item, index) => {
      newMap.set(index, [item[0][0][1], item[1][0][1], item[2][1]]);
    });

    for (let currentItem of newMap.entries()) {
      for (let previousItem of previousMap.entries()) {
        if (
          previousItem[0] === currentItem[0] &&
          previousItem[1][2] === currentItem[1][2]
        ) {
          let trendArray = currentItem[1];
          trendArray.splice(3, 1, "stagnating  \u{23E9}");
          newMap.set(currentItem[0], trendArray);
        }
        if (
          previousItem[0] === currentItem[0] &&
          previousItem[1][2] > currentItem[1][2]
        ) {
          let trendArray = currentItem[1];
          trendArray.splice(3, 1, "declining  \u{23EC}");
          newMap.set(currentItem[0], trendArray);
        }
        if (
          previousItem[0] === currentItem[0] &&
          previousItem[1][2] < currentItem[1][2]
        ) {
          let trendArray = currentItem[1];
          trendArray.splice(3, 1, "growing  \u{23EB}");
          newMap.set(currentItem[0], trendArray);
        }
      }
    }
    let finalArray = [];
    for (let item of newMap.entries()) {
      finalArray.push(item[1]);
    }
    props.saveTrendPairs(finalArray);
  }, [ratesDataArray]);

  useEffect(() => {
    let configurationArray = [];
    axios
      .get("http://localhost:3000/configuration")
      .then(response => {
        let pairsObject = response.data.currencyPairs;
        let allIds = [];
        allIds = Object.keys(pairsObject);
        for (let item in pairsObject) {
          let double = pairsObject[item];
          configurationArray.push([
            item,
            Object.entries(double[0]),
            Object.entries(double[1])
          ]);
        }
        return allIds;
      })
      .then(allIds => {
        setErrormessage(null);
        props.saveConfigurationData(allIds);
        props.saveFilterConfiguration(configurationArray);
        setInterval(() => fetchAllRates(allIds), 5000);
      })
      .catch(err => {
        console.log("Error configuration fetch", err);
        setErrormessage(err.toString());
      });
  }, []);

  const fetchAllRates = allIds => {
    let queryString =
      "?currencyPairIds[]=" +
      allIds.toString().replace(/,/g, "&currencyPairIds[]=");
    axios
      .get(`http://localhost:3000/rates${queryString}`)
      .then(response => {
        setErrormessage(null);
        setRatesData(Object.entries(response.data.rates));
      })
      .catch(err => {
        console.log("Error 500 fetch rates", err);
        setErrormessage(err.toString());
      });
  };

  return (
    <div className={styles.currency__wrap}>
      {errormessage ? (
        <div className={styles.empty__box}>
          <h3 className={styles.info__text}>{errormessage}</h3>
        </div>
      ) : null}
      {props.trendPairs && props.trendPairs.length > 0 ? (
        <div className={styles.currency__container}>
          <TransitionGroup className={styles.dash__display}>
            {props.trendPairs.map((item, index) => (
              <CSSTransition
                key={index}
                appear={true}
                timeout={500}
                classNames={{
                  enter: styles["anim-enter"],
                  enterActive: styles["anim-enter-active"],
                  exit: styles["anim-exit"],
                  exitActive: styles["anim-exit-active"]
                }}
              >
                <div className={styles.dash__currency__box}>
                  <p className={styles.currency__name}>{item[0]}</p>{" "}
                  <p className={styles.currency__name}>{item[1]}</p>{" "}
                  <p className={styles.currency__rate}>{item[2]}</p>{" "}
                  <p className={styles.currency__symbol}>{item[3]}</p>{" "}
                </div>
              </CSSTransition>
            ))}
          </TransitionGroup>
        </div>
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
)(CurrencyDisplay);
