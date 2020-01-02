import React, { useEffect } from "react";
import { connect } from "react-redux";
import { ConfigDispatch } from "../../redux/configuration/configuration.models";
import { fetchConfigAsync } from "../../redux/configuration/configuration.actions";
import { searchCurrency } from "../../redux/filter/filter.actions";
import { fetchRatesAsync } from "../../redux/rates/rates.actions";
import RateName from "../rate/rate-name.component";
import RateTrend from "../rate/rate-trend.component";
import { getFilteredCurrencies } from "../../redux/filter/filter.selectors";
import { saveState } from "../../utils";
import Alert from "../alert/alert.component";
import Select from "../select/select.component";
import Spinner from "react-spinkit";
import { RootState, Props } from "../../types";
import "./styles.module.css";
import { toast } from "react-toastify";

const INTERVAL = 10000;

const RatesList: React.FC<Props> = props => {
  const {
    fetchConfig,
    fetchRates,
    rates,
    config,
    searchCurrency,
    isError,
    loadingConfig,
    searchTerm
  } = props;

  useEffect(() => {
    const { fetchConfig } = props;
    fetchConfig();
  }, [fetchConfig]);

  useEffect(() => {
    const interval = setInterval(() => {
      fetchRates();
    }, INTERVAL);
    isError && notify();
    return () => clearInterval(interval);
  }, [fetchRates, isError]);

  const handleChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const { value } = e.target;
    saveState("select", value);
    searchCurrency(value);
  };

  const notify = () => toast.error("500 Internal Server Error!!");

  return (
    <>
      <Alert />
      <Select handleChange={handleChange} value={searchTerm} />
      {loadingConfig ? (
        <div className="rates-container">
          <Spinner name="ball-spin-fade-loader" />
        </div>
      ) : (
        <div className="rates-container">
          {Object.keys(config).map(id => {
            return (
              <div className="rate-container" key={id}>
                <RateName currency={config[id]} />
                {rates[id] ? (
                  <div className="trend-container">
                    <RateTrend
                      rate={rates[id] && rates[id].currentRate}
                      trend={rates[id] && rates[id].trend}
                    />
                  </div>
                ) : (
                  <div className="trend-container">
                    <Spinner name="circle" />
                  </div>
                )}
              </div>
            );
          })}
        </div>
      )}
    </>
  );
};

const mapStateToProps = (state: RootState) => {
  return {
    config: getFilteredCurrencies(state),
    loadingConfig: state.configuration.isLoading,
    rates: state.rates.ratesList,
    isError: state.rates.showErrorAlert,
    searchTerm: state.filter.searchTerm
  };
};

const mapDispatchToProps = (dispatch: ConfigDispatch) => {
  return {
    fetchConfig: () => dispatch(fetchConfigAsync()),
    fetchRates: () => dispatch(fetchRatesAsync()),
    searchCurrency: (value: string) => dispatch(searchCurrency(value))
  };
};

export default connect(mapStateToProps, mapDispatchToProps)(RatesList);
