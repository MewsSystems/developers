import {ConfigurationData} from '../redux/configuration/configuration.models'
import {RatesData} from '../redux/rates/rates.model'

export interface RootState {
  configuration?: {
    currencies: ConfigurationData,
    isLoading: boolean,
    error: string
  },
  rates?: {
    ratesList: RatesData,
    isLoading: boolean,
    error: string,
  },
  filter: {
    searchTerm: string
  }
}