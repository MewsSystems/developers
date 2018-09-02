import { allPass, compose, endsWith, F, ifElse, lt, not, startsWith } from 'ramda';

export const parseNumber = (value) => Number(value);

export const numberIsPositiveInteger = ifElse(Number.isInteger, lt(0), F);

export const stringIsPositiveInteger = allPass([
    compose(not, endsWith('.')),
    compose(not, startsWith('.')),
    compose(numberIsPositiveInteger, parseNumber),
]);
