// @flow

const StoreFilter = (filter: string) => {
  localStorage.setItem('xChangeFilterMews', filter);
};

const ClearFilter = () => {
  localStorage.removeItem('xChangeFilterMews');
};

const GetFilter = () => {
  const filter = localStorage.getItem('xChangeFilterMews');
  return filter;
};

const GetPairs = () => {
  const localPairs: any = localStorage.getItem('xChangePairsMews');
  return JSON.parse(localPairs);
};

export { StoreFilter, ClearFilter, GetFilter, GetPairs };
