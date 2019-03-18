import { Currency } from './currency.model';
import { Status } from './status.model';

export class ExchangeRate {
    id: string;
    sourceCurrency: Currency;
    targetCurrency: Currency;
    rate: number;
    status: Status;
}
