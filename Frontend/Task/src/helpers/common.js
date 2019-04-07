export const getRatesFromApi = (selected, getSelectedRates) => {
  if (selected && selected.length > 0) {
    getSelectedRates(selected.map(a => a.value));
  }
};

export const getLabel = pair => {
  const [from, to] = pair;

  return `${from.code} / ${to.code}`;
};

export const getOptions = data => Object.entries(data)
  .map(([key, keyValue]) => ({
    label: getLabel(keyValue),
    value: key,
  }));
