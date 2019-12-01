import React from "react";

import { getConfigDTO, getRatesDTO } from "../services/RatesService";
import LocalStorageService from "../services/LocalStorageService";
import {
  loadConfigAction,
  setConfigLoaded,
  saveRatesAction,
  setFirstRatesLoaded,
  updateRatesAction,
  loadConfigLocalStorageAction
} from "../store/Actions";
import { interval } from "../config.json";
import { StoreShape } from "../models/StoreShape";
import { Dispatch } from "redux";
import { connect, ConnectedProps } from "react-redux";
import Currency from "../models/Currency";
import { LocalStorageDTO } from "../models/DTOs";

/* const sleep = (m: number) => new Promise(r => setTimeout(r, m)); */

interface LoaderState {
  intervalId: number | undefined;
}
/**
 * Used as a loader, with Children components
 */
class Loader extends React.Component<LoaderProps, LoaderState> {
  constructor(props: LoaderProps) {
    super(props);
    this.state = {
      intervalId: undefined
    };
  }
  /**Used to load the /congiguration - ids and currency pairs. Returns array of ids of currency pairs*/
  async saveConfig(): Promise<string[]> {
    try {
      const configDTO = await getConfigDTO();
      const { loadConfigActionDispatch, setConfigLoadedDispatch } = this.props;
      loadConfigActionDispatch(configDTO);
      setConfigLoadedDispatch(true);
      /**Backup data to LS */
      LocalStorageService.backup(this.props.currencyPairs);
      return Object.keys(configDTO);
    } catch (e) {
      return this.saveConfig();
    }
  }
  /**Saves local storage object to store and returns currency pair Ids */
  saveLocalStorageConfig(localStorageDTO: LocalStorageDTO): string[] {
    const {
      loadConfigLocalStorageDispatch,
      setConfigLoadedDispatch
    } = this.props;
    loadConfigLocalStorageDispatch(localStorageDTO);
    setConfigLoadedDispatch(true);
    return Object.keys(localStorageDTO);
  }
  /**Used to save rates on initial /rates call */
  async saveRates(ids: string[]) {
    try {
      let ratesDTO = await getRatesDTO(ids);
      const {
        saveRatesActionDispatch,
        setFirstRatesLoadedDispatch
      } = this.props;
      saveRatesActionDispatch(ratesDTO);
      setFirstRatesLoadedDispatch(true);
      return ratesDTO;
    } catch (e) {
      this.saveRates(ids);
    }
  }
  /**Used to update rates on second etc. /rates call */
  async updateRates() {
    try {
      var ids: string[] = Object.keys(this.props.currencyPairs);
      let ratesDTO = await getRatesDTO(ids);
      const { updateRatesActionDispatch } = this.props;
      updateRatesActionDispatch(ratesDTO);
    } catch (e) {
      this.updateRates();
    }
  }

  setPeriodicRatesUpdates() {
    this.setState({
      intervalId: window.setInterval(() => {
        this.updateRates();
      }, interval)
    });
  }
  /**
   * Loading logic applied here, distinguish between cases, where LocalStorage was used or not
   */
  componentDidMount() {
    if (!this.props.configLoaded) {
      var localStorageConfig = LocalStorageService.load();

      if (localStorageConfig === false) {
        this.saveConfig()
          .then(ids => {
            if (ids !== undefined) {
              return this.saveRates(ids);
            }
          })
          .then(() => this.setPeriodicRatesUpdates());
      } else {
        var DTO = this.saveLocalStorageConfig(localStorageConfig);
        this.saveRates(DTO).then(() => this.setPeriodicRatesUpdates());
      }
    }
  }
  /**Unsets the interval */
  componentWillUnmount() {
    if (this.state.intervalId !== undefined) {
      window.clearInterval(this.state.intervalId);
    }
  }

  /**Renders the content of the application*/
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
    },
    loadConfigLocalStorageDispatch: (payload: LocalStorageDTO) => {
      dispatch(loadConfigLocalStorageAction(payload));
    }
  };
};

const connector = connect(mapStateToProps, mapDispatchToProps);
export type LoaderProps = ConnectedProps<typeof connector>; //Prop type of Loader
export default connector(Loader);
