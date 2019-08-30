import React from 'react';
import Adapter from 'enzyme-adapter-react-16';
import { shallow, configure } from 'enzyme';
import configureMockStore from 'redux-mock-store';
import { Provider } from 'react-redux';
import Filter from '../../components/Filter/Filter';
import { findByTestAtrr, checkProps } from '../../utils';

const mockStore = configureMockStore();
const store = mockStore({});

const setUp = (props = {}) => {
  const component = shallow(
    <Provider store={store}>
      <Filter {...props} />
    </Provider>,
  );
  return component;
};

configure({ adapter: new Adapter() });

/**
 * [checks if component is rendering properly with given props]
 * @type {Object}
 */
describe('Filter Component', () => {
  describe('Checking PropTypes', () => {
    it('Should not throw a warning', () => {
      const expectedProps = {
        selectOptions: [],
        rateId: [],
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
        rateId: '1',
      };
      wrapper = setUp(props);
    });

    it('Should render without errors', () => {
      const component = findByTestAtrr(wrapper, 'filterComponent');
      expect(component.length).toBe(0);
    });
  });
});
