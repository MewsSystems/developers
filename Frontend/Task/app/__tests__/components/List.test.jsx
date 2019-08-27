import React from 'react';
import Adapter from 'enzyme-adapter-react-16';
import { shallow, configure } from 'enzyme';
import configureMockStore from 'redux-mock-store';
import { Provider } from 'react-redux';
import List from '../../components/List/List';
import { findByTestAtrr, checkProps } from '../../utils';

const mockStore = configureMockStore();
const store = mockStore({});

const setUp = (props = {}) => {
  const component = shallow(<Provider store={store}>
    <List {...props} />
  </Provider>);
  return component;
};

configure({ adapter: new Adapter() });

/**
 * [checks if component is rendering properly with given props]
 * @type {Object}
 */
describe('List Component', () => {
    describe('Checking PropTypes', () => {
        it('Should not throw a warning', () => {
            const expectedProps = {
							isLoadingConfiguration: true,
							isLoadingRates: true,
							rates: [],
							status: 200,
							rateId: [],
            };
            const propsErr = checkProps(List, expectedProps);
            expect(propsErr).toBeUndefined();
        });
    });
		describe('Have props', () => {
			 let wrapper;
			 beforeEach(() => {
					 const props = {
						 isLoadingConfiguration: true,
						 isLoadingRates: true,
						 rates: [],
						 status: 200,
						 rateId: 'asduasuhdahus',
					 };
					 wrapper = setUp(props);
			 });

			 it('Should render without errors', () => {
					 const component = findByTestAtrr(wrapper, 'listComponent');
					 expect(component.length).toBe(0);
			 });
	 });
});
