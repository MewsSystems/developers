import reducer from '../../reducers/CurrencyReducer';
import * as types from '../../actions/types';
import {
 parseDate,
} from '../../utils/index';


/**
 * [checks if reducer is working as expected with given state and action type.]
 * @type {Array}
 */
describe('Currency Reducer', () => {
	it('should return the initial state', () => {
    expect(reducer(undefined, {})).toEqual({
			configuration: [],
	 	 	rates: [],
	 	 	isLoadingConfiguration: true,
	 	 	isLoadingRates: true,
	 	 	status: 200,
	 	 	request: '',
	 	 	timestamp: '',
			rateId: [],
		});
  });

	it('GET_CONFIGURATION_SUCCESS', () => {
		expect(
      reducer([], {
        type: types.GET_CONFIGURATION_SUCCESS,
				configuration: {
		    '70c6744c-cba2-5f4c-8a06-0dac0c4e43a1': [
		      {
		        code: 'AMD',
		        name: 'Armenia Dram',
		      },
		      {
		        code: 'GEL',
		        name: 'Georgia Lari',
		      },
		    ],
				},
      }),
    ).toEqual(
      {
				configuration: {
		    '70c6744c-cba2-5f4c-8a06-0dac0c4e43a1': [
		      {
		        code: 'AMD',
		        name: 'Armenia Dram',
		      },
		      {
		        code: 'GEL',
		        name: 'Georgia Lari',
		      },
		    ],
				},
				isLoadingConfiguration: false,
				status: 200,
      },
    );
  });

	it('GET_RATE_SUCCESS', () => {
	 expect(
			reducer([], {
				type: types.GET_RATE_SUCCESS,
				rates: [],
			}),
		).toEqual(
			{
				rates: [],
				isLoadingRates: false,
				status: 200,
				timestamp: parseDate(new Date()),
			},
		);
	});
	it('RESPONSE_ERROR', () => {
		expect(
 			reducer([], {
 				type: types.RESPONSE_ERROR,
				status: 200,
 			}),
 		).toEqual(
 			{
 				status: 200,
 			},
 		);
	});
	it('REQUEST_ERROR', () => {
		expect(
 			reducer([], {
 				type: types.REQUEST_ERROR,
				request: 'Oopps',
 			}),
 		).toEqual(
 			{
 				request: 'Oopps',
 			},
 		);
	});
	it('FILTER', () => {
		expect(
 			reducer([], {
 				type: types.FILTER,
				rateId: '70c6744c-cba2-5f4c-8a06-0dac0c4e43a1',
 			}),
 		).toEqual(
 			{
 				rateId: '70c6744c-cba2-5f4c-8a06-0dac0c4e43a1',
 			},
 		);
	});
});
