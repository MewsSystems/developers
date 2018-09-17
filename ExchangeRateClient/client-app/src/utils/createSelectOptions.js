// @flow strict

// TODO name ad to const
const DEFAULT_SELECTED_COUNT = 3;

export const sanitizeConfiguration = ({
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
    (acc, id, index) => ({
      ...acc,
      [id]: { currencies: currencyPairs[id], isSelected: index < DEFAULT_SELECTED_COUNT && true },
    }),
    {},
  );

  const result = {
    selectedRates: ids,
    data: temp,
  };
  return result;
};
