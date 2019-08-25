//set item to localStorage
export const set = (key, value) => {
	localStorage.setItem(key, JSON.stringify(value));
}
//get item from localStorage
export const get = (key) => {
   return JSON.parse(localStorage.getItem(key));
}

//checking trend
export const checkTrend = (oldValue, newValue) => {
  if (oldValue > newValue) {
      return 'declining'
  }
  if (newValue > oldValue) {
      return 'growing'
  }
  return 'same';
};

//return new rates
export const compareRates = (oldRates,newRates) => {
  for (let i = 0; i < newRates.length; i++) {
      const newRate = newRates[i];
      const oldRate = oldRates.find(entry => entry.id === newRate.id);

      if (oldRate) {
         newRates[i].type = checkTrend(oldRate.value, newRate.value);
      }
  }
  return newRates;
};
