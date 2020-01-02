import { ConfigurationData } from "../redux/configuration/configuration.models";
import { RatesData } from "../redux/rates/rates.model";
export interface RootState {
  configuration: {
    currencies: ConfigurationData;
    isLoading: boolean;
    errorMessage: string;
  };
  rates: {
    ratesList: RatesData;
    isLoading: boolean;
    errorMessage: string;
    showErrorAlert: boolean;
  };
  filter: {
    searchTerm: string;
  };
}

export interface Props {
  fetchConfig: () => void;
  fetchRates: () => void;
  searchCurrency: (value: string) => void;
  isError: boolean;
  loadingConfig: boolean;
  searchTerm: string;
  rates: RatesData;
  config: ConfigurationData;
}
