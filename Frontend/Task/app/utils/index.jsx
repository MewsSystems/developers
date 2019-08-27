import checkPropTypes from 'check-prop-types';

/**
 * [set sets to localstorage with key,value pairs]
 * @param {[String]} key   [localstorage item name]
 * @param {[Object]} value [value to be stored]
 */
export const set = (key, value) => {
	localStorage.setItem(key, JSON.stringify(value));
};

/**
 * [get gets item from localstorage with specific key]
 * @param  {[String]} key [localstorage item to be retrieved]
 * @return {[Object]}     [value of the key]
 */
export const get = (key) => {
    const serializeData = localStorage.getItem(key);
    if (serializeData === null) return [];
    return JSON.parse(serializeData);
};

/**
 * [checkTrend description]
 * @param  {[Number]} oldValue [old value to be compared]
 * @param  {[Number]} newValue [new value to be compared]
 * @return {[String]}          [description]
 */
export const checkTrend = (oldValue, newValue) => {
  if (oldValue > newValue) {
      return 'declining';
  }
  if (newValue > oldValue) {
      return 'growing';
  }
  return 'same';
};

/**
 * [parseDate description]
 * @param  {[Object]} date [date object to be parsed]
 * @return {[String]}      [human readable timestamp]
 */
export const parseDate = (date) => {
	let hours = date.getHours();
	let minutes = `0${date.getMinutes()}`;
	let seconds = `0${date.getSeconds()}`;

	let formattedTime = `${hours}:${minutes.substr(-2)}:${seconds.substr(-2)}`
	return formattedTime;
};

/**
 * [findByTestAtrr function for testing components]
 * @param  {[Object]} component [component to check for test]
 * @param  {[String]} attr      [the attr attribute to be checked]
 * @return {[Object]}           [wrapper]
 */
export const findByTestAtrr = (component, attr) => {
  const wrapper = component.find(`[data-test='${attr}']`);
  return wrapper;
};

/**
 * [checkProps function for testing props]
 * @param  {[Function]} component     [function that checks component]
 * @param  {[Object]} expectedProps [props to be checked for validation]
 * @return {[type]}               [undefined if fails]
 */
export const checkProps = (component, expectedProps) => {
    const propsErr = checkPropTypes(component.propTypes, expectedProps, 'props', component.name);
    return propsErr;
};
