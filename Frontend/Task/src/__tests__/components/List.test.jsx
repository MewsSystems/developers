import React from 'react';
import Adapter from 'enzyme-adapter-react-16';
import { shallow, configure } from 'enzyme';
import List from '../../components/List/List';
import { findByTestAtrr, checkProps } from '../../utils';

const setUp = (props = {}) => {
  const component = shallow(<List {...props} />);
  return component;
};

configure({ adapter: new Adapter() });

describe('List Component', () => {
    describe('Checking PropTypes', () => {
        it('Should not throw a warning', () => {
            const expectedProps = {
							isLoadingConfiguration: true,
							isLoadingRates: true,
							rates: [],
							status: 200,
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
					 };
					 wrapper = setUp(props);
			 });

			 it('Should render without errors', () => {
					 const component = findByTestAtrr(wrapper, 'listComponent');
					 expect(component.length).toBe(1);
			 });
	 });
});
