import checkPropTypes from 'check-prop-types';

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

// checking trend for type
export const checkTrend = (oldValue, newValue) => {
  if (oldValue > newValue) {
      return 'declining';
  }
  if (newValue > oldValue) {
      return 'growing';
  }
  return 'same';
};

export const parseDate = (date) => {
	let hours = date.getHours();
	let minutes = `0${date.getMinutes()}`;
	let seconds = `0${date.getSeconds()}`;

	let formattedTime = `${hours}:${minutes.substr(-2)}:${seconds.substr(-2)}`

	return formattedTime;
};

// component testing
export const findByTestAtrr = (component, attr) => {
  const wrapper = component.find(`[data-test='${attr}']`);
  return wrapper;
};

export const checkProps = (component, expectedProps) => {
    const propsErr = checkPropTypes(component.propTypes, expectedProps, 'props', component.name);
    return propsErr;
};
