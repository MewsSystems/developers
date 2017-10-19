import React from 'react';
import PropTypes from 'prop-types';
import {connect} from 'react-redux';
import sn from 'classnames';
import '../styles/Button.less'; //enforce style build order
import s from '../styles/Filter.less';
import Button from './Button';
import {getViewPairs, getFilter} from '../selectors';
import {onFilterChanged} from '../actions';


const Filter = ({items, disabled, onItemClick}) => (
  <div className={sn(s.filter)}>
    {items.map(item => (
      <Button
        key={item.id}
        className={sn(s.btn, {[s.btnOff]: ~disabled.indexOf(item.id)})}
        type='button'
        text={item.name}
        onClick={() => onItemClick(item.id)}
      />
    ))}
  </div>
);

Filter.propTypes = {
  items: PropTypes.arrayOf(PropTypes.shape({
    id: PropTypes.string,
    name: PropTypes.string
  })),
  disabled: PropTypes.arrayOf(PropTypes.string),
  onItemClick: PropTypes.func,
};

Filter.defaultProps = {
  items: [],
  disabled: [],
  onItemClick: e => {},
};

export default connect(state => ({
  items: getViewPairs(state),
  disabled: getFilter(state)
}), {
  onItemClick: onFilterChanged
})(Filter);
