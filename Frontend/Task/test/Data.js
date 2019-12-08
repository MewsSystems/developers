import { CurrencyPair } from "../app/data/CurrencyPair";

export const CURRENCY_AMD = {
    code: "AMD",
    name: "Armenia Dram"
};

export const CURRENCY_PHP = {
    code: "PHP",
    name: "Philippines Peso"
};

export const CURRENCY_PAIR_1 = new CurrencyPair();
CURRENCY_PAIR_1.id = "1";
CURRENCY_PAIR_1.frcode = CURRENCY_AMD;
CURRENCY_PAIR_1.tocode = CURRENCY_PHP;

export const CURRENCY_PAIR_2 = new CurrencyPair();
CURRENCY_PAIR_2.id = "2";
CURRENCY_PAIR_2.frcode = CURRENCY_PHP;
CURRENCY_PAIR_2.tocode = CURRENCY_AMD;

export const CURRENCY_PAIR_3 = new CurrencyPair();
CURRENCY_PAIR_3.id = "3";
CURRENCY_PAIR_3.frcode = CURRENCY_AMD;
CURRENCY_PAIR_3.tocode = CURRENCY_AMD;

export const CURRENCY_PAIRS_BY_IDS = {};
CURRENCY_PAIRS_BY_IDS[CURRENCY_PAIR_1.id] = CURRENCY_PAIR_1;
CURRENCY_PAIRS_BY_IDS[CURRENCY_PAIR_2.id] = CURRENCY_PAIR_2;
CURRENCY_PAIRS_BY_IDS[CURRENCY_PAIR_3.id] = CURRENCY_PAIR_3;

export const getCopyOfCurrencyPairsById = () => {
    const currencyPairsById = {};
    currencyPairsById[CURRENCY_PAIR_1.id] = CURRENCY_PAIR_1.clone();
    currencyPairsById[CURRENCY_PAIR_2.id] = CURRENCY_PAIR_2.clone();
    currencyPairsById[CURRENCY_PAIR_3.id] = CURRENCY_PAIR_3.clone();
    return currencyPairsById;
}

export const ENDPOINT = "localhost";
export const INTERVAL = 10000;
export const RATE_1 = 3.56;
export const INC_RATE_1 = { trend: "^", value: RATE_1 + 1.0 };
export const RATE_2 = 4.5;
export const DEC_RATE_2 = { trend: "v", value: RATE_2 - 1.0 };
export const RATE_3 = 5.0000000000000000001;
export const STG_RATE_3 = { trend: "=", value: RATE_3 };
export const RATES_1 = {};
RATES_1[CURRENCY_PAIR_1.id] = RATE_1;
RATES_1[CURRENCY_PAIR_2.id] = RATE_2;
RATES_1[CURRENCY_PAIR_3.id] = RATE_3;
export const RATES_2 = {};
RATES_2[CURRENCY_PAIR_1.id] = RATE_1 + 1.0;
RATES_2[CURRENCY_PAIR_2.id] = RATE_2 - 1.0;
RATES_2[CURRENCY_PAIR_3.id] = 5.0000000000000000001;
