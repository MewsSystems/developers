import React, { useEffect, Fragment } from "react";
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
import { RootState } from "../../types";
import "./styles.module.css";
import { toast } from "react-toastify";

const INTERVAL = 10000;

interface Props {
  fetchConfig: () => void;
  fetchRates: () => void;
  searchCurrency: (value: string) => void;
  isError: boolean;
  loadingConfig: boolean;
  searchTerm: string;
  rates: any;
}

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
  console.log("config", config);
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
    saveState("select", e.target.value);
    searchCurrency(e.target.value);
  };

  const notify = () => toast.error("500 Internal Server Error!!");

  return (
    <>
      <Alert />
      <Select handleChange={handleChange} value={searchTerm} />
      {loadingConfig ? (
        <Spinner name="ball-spin-fade-loader" />
      ) : (
        <div className="rates-container">
          {Object.keys(config).map((id, i) => {
            return (
              <div className="rate-wrapper" key={id}>
                <RateName currency={config[id]} />
                {rates[i] ? (
                  <RateTrend
                    rate={rates[i] && rates[i].rate}
                    trend={rates[i] && rates[i].trend}
                  />
                ) : (
                  <Spinner name="circle" />
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
