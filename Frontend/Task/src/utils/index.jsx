// set item to localStorage
export const set = (key, value) => {
	localStorage.setItem(key, JSON.stringify(value));
};
// get item from localStorage
export const get = (key) => {
    const serializeData = localStorage.getItem(key);
    if (serializeData === null) return [];
    return JSON.parse(serializeData);
};

// checking trend
export const checkTrend = (oldValue, newValue) => {
  if (oldValue > newValue) {
      return 'declining';
  }
  if (newValue > oldValue) {
      return 'growing';
  }
  return 'same';
};
