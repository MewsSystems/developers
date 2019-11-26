import "react";
import React from "react";
import { StoreShape } from "../models/StoreShape";
import { connect, ConnectedProps } from "react-redux";
import { CurrencyRates } from "./CurrencyRates";
import { Dispatch } from "redux";
import { togglePairVisibilityAction } from "../store/Actions";
import LocalStorageService from "../services/LocalStorageService";

import { store } from "../store/store";

const mapStateToProps = (state: StoreShape) => ({
  currencyPairs: state.currencyPairs,
  configLoaded: state.configLoaded,
  firstRatesLoaded: state.firstRatesLoaded
});

const mapDispatchToProps = (dispatch: Dispatch) => ({
  toggleVisibility: (id: string) => {
    dispatch(togglePairVisibilityAction(id));
    LocalStorageService.backup(store.getState().currencyPairs);
  }
});

const connector = connect(mapStateToProps, mapDispatchToProps);
export type PropsFromRedux = ConnectedProps<typeof connector>;

export default connector(CurrencyRates);
