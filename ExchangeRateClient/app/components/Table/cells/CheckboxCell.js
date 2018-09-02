import React from 'react';
import PropTypes from 'prop-types';
import Input from '../../Input';

const CheckboxCell = ({ defaultValue, onChange }) =>
    <td align="center">
        <Input type="checkbox" onChange={onChange} checked={defaultValue} />
    </td>;

CheckboxCell.propTypes = {
    defaultValue: PropTypes.bool,
    onChange: PropTypes.func,
};

export default CheckboxCell;
