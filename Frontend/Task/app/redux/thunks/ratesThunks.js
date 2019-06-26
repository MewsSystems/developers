import { ratesActionCreators } from "../actions/ratesActions";
import RatesService from '../../services/RatesService';

export default {
  updateRates: (currencyPairsIds) => async (dispatch) => {
    dispatch(ratesActionCreators.updateRates());

    try {
      const rates = await RatesService.updateRates(currencyPairsIds);
      dispatch(ratesActionCreators.updateRatesSuccess(rates));
    } catch (e) {
      dispatch(ratesActionCreators.updateRatesFailed());
    }
  },
  filterRates: (filter) => (dispatch) => {
    dispatch(ratesActionCreators.filterRates(filter));
  }

};