import { FETCH_CONFIGURATION, HANDLE_SELECTED_PRODUCTS, HANDLE_PRODUCTS } from './types';

export const fetchConfiguration = () => dispatch => {
	fetch('http://localhost:3000/configuration')
	.then(response => response.json())
	.then(json => dispatch({
			type: FETCH_CONFIGURATION,
			payload: json
		}))
	.then(() => console.log('configuration loaded'));
};

export const handleSelectedProducts = (selectedProductIds) => dispatch => {
	dispatch({
		type: HANDLE_SELECTED_PRODUCTS,
		payload: selectedProductIds
	});
};

export const handleProducts = (products) => dispatch => {
	dispatch({
		type: HANDLE_PRODUCTS,
		payload: products
	});
};

