import styled from 'styled-components';

const selector = (key, defaultValue) => {
	return props => {
		const has = props.hasOwnProperty(key);
		if (has) return props[key];
		return defaultValue
	}
} 

const createLayout = direction => styled ('div')`
	display: flex;
	flex-direction: ${direction};
	align-items: ${selector('align', 'flex-start')};
	justify-content: ${selector('justify', 'flex-start')};
`;

const Row = createLayout ('row');
const Column = createLayout ('column');

export {Row, Column};
