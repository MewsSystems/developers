import React from 'react';
import Adapter from 'enzyme-adapter-react-16';
import { shallow, configure } from 'enzyme';
import Filter from '../../components/Filter/Filter';
import { findByTestAtrr, checkProps } from '../../utils';

const setUp = (props = {}) => {
  const component = shallow(<Filter {...props} />);
  return component;
};

configure({ adapter: new Adapter() });

describe('Filter Component', () => {
    describe('Checking PropTypes', () => {
        it('Should not throw a warning', () => {
            const expectedProps = {
							selectOptions: [],
            };
            const propsErr = checkProps(Filter, expectedProps);
            expect(propsErr).toBeUndefined();
        });
    });
		describe('Have props', () => {
			 let wrapper;
			 beforeEach(() => {
					 const props = {
							selectOptions: [],
					 };
					 wrapper = setUp(props);
			 });

			 it('Should render without errors', () => {
					 const component = findByTestAtrr(wrapper, 'filterComponent');
					 expect(component.length).toBe(1);
			 });
	 });
});
