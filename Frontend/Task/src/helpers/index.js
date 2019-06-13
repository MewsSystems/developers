import _ from 'lodash';

export const getShortcut = (currencyList) => {
  return currencyList && currencyList.length ? currencyList.map(y => y.name).join('/') : '-';
};

export const getCurrencies = (data) => {
  let currencies = _.uniqBy(_.flatten(Object.keys(data).map(x => data[x])), 'code');
  return currencies;
};