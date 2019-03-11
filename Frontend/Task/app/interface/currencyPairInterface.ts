import { CurrencyInterface } from './currencyInterface';

export enum Trend {
    GROWING = 'growing',
    DECLINING = 'declining',
    STAGNATING = 'stagnating',
}

export default interface CurrencyPairInterface {
    pair: CurrencyInterface[];
    rate?: number;
    trend?: string;
    shortcut?: string;
}
