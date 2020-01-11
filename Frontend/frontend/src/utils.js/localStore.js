const LS__RATES_CONFIGURATION = 'rates_configuration';
const LS__RATES_FILTERS = 'rates_filters';


/**
 * Is supported LocalStorage
 */
export const isSupportedLS = () => {
  if (typeof Storage !== 'undefined') {
    return true;
  }
  return false;
};


/* ---------------------------------------- */
/* -------- Rates Configuration              */
/* ---------------------------------------- */
export const saveRatesConfigurationLS = (rates) => {
  if (!isSupportedLS()) return;
  localStorage.setItem(LS__RATES_CONFIGURATION, JSON.stringify(rates));
};

export const getRatesConfigurationLS = () => {
  if (!isSupportedLS()) return null;
  return JSON.parse(localStorage.getItem(LS__RATES_CONFIGURATION));
};


/* ---------------------------------------- */
/*         Rates Filter                     */
/* ---------------------------------------- */
