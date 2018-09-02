import React from 'react';
import PropTypes from 'prop-types';
import { FormGroup, Input } from 'reactstrap';

const SelectNumberOfRows = ({ handleChange }) =>
    <FormGroup>
        <Input type="select" onChange={handleChange}>
            <option>5</option>
            <option>10</option>
            <option>20</option>
            <option>50</option>
            <option>100</option>
        </Input>
    </FormGroup>;

SelectNumberOfRows.propTypes = {
    handleChange: PropTypes.func,
};

export default SelectNumberOfRows;
