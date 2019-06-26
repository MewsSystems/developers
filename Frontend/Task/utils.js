export const buildCurrencyPairsIdsQuery = (ids) => {
  const pairs = [];
  ids.forEach((id) => pairs.push(`currencyPairIds[]=${id}`));
  return pairs.join('&');
};