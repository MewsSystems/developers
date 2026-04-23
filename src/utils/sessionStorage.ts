export const loadState = () => {
  try {
    const serializedState = sessionStorage.getItem('mews-movie-search');
    if (serializedState === null) {
      return undefined;
    }
    return JSON.parse(serializedState);
  } catch (err) {
    return undefined;
  }
};

export const saveState = (state: any) => {
  try {
    const serializedState = JSON.stringify({ ...state });
    sessionStorage.setItem('mews-movie-search', serializedState);
  } catch (err) {
    // Ignore write errors.
  }
};
