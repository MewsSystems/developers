import React from "react";
import { store } from "../store/store";
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
import { interval, endpoint } from "../../config.json";
import { StoreShape } from "../models/StoreShape";
import { Dispatch } from "redux";
import { connect, ConnectedProps } from "react-redux";
import Currency from "../models/Currency";
import { LocalStorageDTO } from "../models/DTOs";

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
  async saveConfig(): Promise<string[]> {
    try {
      const configDTO = await getConfigDTO();
      const { loadConfigActionDispatch, setConfigLoadedDispatch } = this.props;
      loadConfigActionDispatch(configDTO);
      setConfigLoadedDispatch(true);
      LocalStorageService.backup(this.props.currencyPairs);
      return Object.keys(configDTO);
    } catch (e) {
      return this.saveConfig();
    }
  }
  saveLocalStorageConfig(localStorageDTO: LocalStorageDTO): string[] {
    const {
      loadConfigLocalStorageDispatch,
      setConfigLoadedDispatch
    } = this.props;
    loadConfigLocalStorageDispatch(localStorageDTO);
    setConfigLoadedDispatch(true);
    return Object.keys(localStorageDTO);

    console.log(this.props);
  }
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

  componentDidMount() {
    if (!this.props.configLoaded) {
      var localStorageConfig = LocalStorageService.load();

      if (localStorageConfig == false) {
        this.saveConfig().then(record => {
          if (record != undefined) {
            return this.saveRates(record);
          }
        });
      } else {
        var DTO = this.saveLocalStorageConfig(localStorageConfig);
        this.saveRates(DTO);

        /* this.saveLocalStorageConfig(localStorageConfig);
        console.log(this.props); */
      }
    }

    this.setState({
      intervalId: window.setInterval(
        (() => {
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
    },
    loadConfigLocalStorageDispatch: (payload: LocalStorageDTO) => {
      dispatch(loadConfigLocalStorageAction(payload));
    }
  };
};
const connector = connect(mapStateToProps, mapDispatchToProps);
export type LoaderProps = ConnectedProps<typeof connector>;
export default connector(Loader);
