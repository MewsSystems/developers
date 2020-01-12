export const LS__RATES_CONFIGURATION = 'rates_configuration';
export const LS__RATES_FILTERS = 'rates_filters';


/**
 * Is supported LocalStorage
 */
export const isSupportedLS = () => {
  if (typeof Storage !== 'undefined') {
    return true;
  }
  return false;
};


/**
 * Save data to Local Storage
 * @param {String} name
 * @param {Any} data
 */
export const setItemLS = (name, data) => {
  if (!isSupportedLS()) return;
  localStorage.setItem(name, JSON.stringify(data));
};


/**
 * Get data from Local Storage
 * @param {String} name
 */
export const getItemLS = (name) => {
  if (!isSupportedLS()) return null;
  return JSON.parse(localStorage.getItem(name));
};
