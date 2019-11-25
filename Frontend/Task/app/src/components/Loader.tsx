import React from "react";
import { store } from "../store/store";
import { getConfigDTO, getRatesDTO } from "../services/RatesService";
import {
  loadConfigAction,
  setConfigLoaded,
  saveRatesAction,
  setFirstRatesLoaded,
  updateRatesAction
} from "../store/Actions";
import { interval, endpoint } from "../../config.json";
import { StoreShape } from "../models/StoreShape";
import { Dispatch } from "redux";
import { connect, ConnectedProps } from "react-redux";
import Currency from "../models/Currency";

/* const sleep = (m: number) => new Promise(r => setTimeout(r, m)); */

interface LoaderState {
  intervalId: number | undefined;
}
class Loader extends React.Component<LoaderProps, LoaderState> {
  constructor(props: LoaderProps) {
    super(props);
    this.state = {
      intervalId: undefined
    };
  }
  async saveConfig() {
    try {
      const configDTO = await getConfigDTO();
      const { loadConfigActionDispatch, setConfigLoadedDispatch } = this.props;
      loadConfigActionDispatch(configDTO);
      setConfigLoadedDispatch(true);
    } catch (e) {
      this.saveConfig();
    }
  }
  async saveRates() {
    try {
      var ids: string[] = Object.keys(this.props.currencyPairs);

      let ratesDTO = await getRatesDTO(ids);
      const {
        saveRatesActionDispatch,
        setFirstRatesLoadedDispatch
      } = this.props;
      saveRatesActionDispatch(ratesDTO);
      setFirstRatesLoadedDispatch(true);
    } catch (e) {
      this.saveRates();
    }
  }
  async updateRates() {
    console.log("updateRates called");
    try {
      var ids: string[] = Object.keys(this.props.currencyPairs);
      let ratesDTO = await getRatesDTO(ids);
      const { updateRatesActionDispatch } = this.props;
      updateRatesActionDispatch(ratesDTO);
    } catch (e) {
      this.updateRates();
    }
  }

  async componentDidMount() {
    if (!this.props.configLoaded) {
      await this.saveConfig();
    }
    if (!this.props.firstRatesLoaded) {
      await this.saveRates();
    }

    this.setState({
      intervalId: window.setInterval(
        (() => {
          console.log("loading new rates");
          this.updateRates();
        }).bind(this),
        10000
      )
    });
  }

  componentWillUnmount() {
    if (this.state.intervalId !== undefined) {
      window.clearInterval(this.state.intervalId);
    }
  }

  /**Loads the config, loads first rates and then  */
  render() {
    return <div>{this.props.children}</div>;
  }
}

const mapStateToProps = (state: StoreShape) => state;

const mapDispatchToProps = (dispatch: Dispatch) => {
  return {
    loadConfigActionDispatch: (payload: Record<string, Currency[]>) => {
      dispatch(loadConfigAction(payload));
    },
    setConfigLoadedDispatch: (value: Boolean) => {
      dispatch(setConfigLoaded(value));
    },
    saveRatesActionDispatch: (payload: Record<string, Number>) => {
      dispatch(saveRatesAction(payload));
    },
    setFirstRatesLoadedDispatch: (value: Boolean) => {
      dispatch(setFirstRatesLoaded(value));
    },
    updateRatesActionDispatch: (payload: Record<string, Number>) => {
      dispatch(updateRatesAction(payload));
    }
  };
};
const connector = connect(mapStateToProps, mapDispatchToProps);
export type LoaderProps = ConnectedProps<typeof connector>;
export default connector(Loader);
