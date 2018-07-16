import { FETCH_CONFIGURATION, HANDLE_SELECTED_PRODUCTS, HANDLE_PRODUCTS } from '../actions/types';

const initialState = {
	products: [
			{
				id: 1,
				name: "supername",
				cost: 400,
				quantity: 1,
			},
			{
				id: 2,
				name: "myproduct",
				cost: 250,
				quantity: 1,
			},
			{
				id: 3,
				name: "mymumsupernicelongproductname",
				cost: 500,
				quantity: 1,
			},
			{
				id: 4,
				name: "notmyproduct",
				cost: 3250,
				quantity: 1
			},
			{
				id: 5,
				name: "mother-in-law",
				cost: 5,
				quantity: 1
			}
		],
		selectedProductIds: [],
		configuration: null,
}

export default function(state = initialState, action) {
	switch(action.type) {
		case FETCH_CONFIGURATION:
		return {
			...state,
			configuration: action.payload
		}
		case HANDLE_SELECTED_PRODUCTS:
		return {
			...state,
			selectedProductIds: action.payload
		}
		case HANDLE_PRODUCTS:
		return {
			...state,
			products: action.payload
		}
		default:
		return state;
	}
}
