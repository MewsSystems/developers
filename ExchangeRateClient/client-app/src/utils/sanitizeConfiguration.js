// @flow strict

export default ({
  currencyPairs,
}: {
  currencyPairs: {
    [id: string]: {
      code: string,
      name: string,
    },
  },
}) => {
  const ids: string[] = Object.keys(currencyPairs);

  const temp = ids.reduce(
    (acc, id) => ({
      ...acc,
      [id]: { currencies: currencyPairs[id] },
    }),
    {},
  );

  const result = {
    selectedRates: ids,
    data: temp,
  };
  return result;
};
