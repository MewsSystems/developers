import * as actions from '../../actions/currencyActions';
import * as types from '../../actions/types';


/**
 * [checks for each action if it works properly by giving the required parameters]
 * @type {Object}
 */
describe('actions', () => {
  it('should create an action GET_CONFIGURATION_SUCCESS', () => {
    const configuration = {
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
		};
    const expectedAction = {
      type: types.GET_CONFIGURATION_SUCCESS,
      configuration,
    };
    expect(actions.getConfigurationSuccess(configuration)).toEqual(expectedAction);
  });

	it('should create an action GET_RATE_SUCCESS', () => {
    const rates = {
			'0c6744c-cba2-5f4c-8a06-0dac0c4e43a1': 5.413,
		};
    const expectedAction = {
      type: types.GET_RATE_SUCCESS,
      rates,
    };
    expect(actions.getRateSuccess(rates)).toEqual(expectedAction);
  });

	it('should create an action REQUEST_ERROR', () => {
    const error = 'oops';
    const expectedAction = {
      type: types.REQUEST_ERROR,
      error,
    };
    expect(actions.requestError(error)).toEqual(expectedAction);
  });

	it('should create an action RESPONSE_ERROR', () => {
    const status = '500';
    const expectedAction = {
      type: types.RESPONSE_ERROR,
      status,
    };
    expect(actions.responseError(status)).toEqual(expectedAction);
  });

	it('should create an action FILTER', () => {
    const rateId = '0c6744c-cba2-5f4c-8a06-0dac0c4e43a1';
    const expectedAction = {
      type: types.FILTER,
      rateId,
    };
    expect(actions.filter(rateId)).toEqual(expectedAction);
  });
});
