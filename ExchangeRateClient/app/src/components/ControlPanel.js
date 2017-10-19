import React from 'react';
import PropTypes from 'prop-types';
import {connect} from 'react-redux';
import sn from 'classnames';
import s from '../styles/ControlPanel';
import Button from './Button';
import {getIsWatching} from '../selectors';
import {onFilterClear, onWatchToggle} from '../actions';


const ControlPanel = ({isWatching, onFilterClick, onWatchClick}) => (
  <div className={sn(s.controlPanel)}>
      <Button
        className={sn(s.btn)}
        text={isWatching ? 'Pause' : 'Resume'}
        onClick={onWatchClick}
      />
      <Button
        className={sn(s.btn)}
        text='Select all'
        onClick={onFilterClick}
      />
  </div>
);

ControlPanel.propTypes = {
  isWatching: PropTypes.bool,
  onFilterClick: PropTypes.func,
  onWatchClick: PropTypes.func,
};

ControlPanel.defaultProps = {
  isWatching: false,
  onFilterClick: e => {},
  onWatchClick: e => {},
};

export default connect(state => ({
  isWatching: getIsWatching(state)
}), {
  onFilterClick: onFilterClear,
  onWatchClick: onWatchToggle,
})(ControlPanel);
