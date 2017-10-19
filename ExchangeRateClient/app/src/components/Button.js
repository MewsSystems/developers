import React from 'react';
import PropTypes from 'prop-types';
import sn from 'classnames';
import s from '../styles/Button';


const Button = ({text, className, onClick}) => (
  <button
    className={sn(s.btn, className)}
    type='button'
    onClick={onClick}
  >
    {text}
  </button>
);

Button.propTypes = {
  text: PropTypes.string,
  className: PropTypes.string,
  onClick: PropTypes.func
};

Button.defaultProps = {
  text: '',
  className: '',
  onClick: e => {}
};

export default Button;
