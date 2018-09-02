import React from 'react';
import PropTypes from 'prop-types';
import styled from 'styled-components';

const Checkmark = styled.span`
    position: absolute;
    top: 0;
    left: 0;
    height: 24px;
    width: 24px;
    background-color: #eee;
    border-radius: 4px;
    margin-left: auto;
    margin-right: auto;
    
    &:after {
        content: "";
        position: absolute;
        display: none;
    }
`;

const CheckboxContainer = styled.label`
    display: block;
    position: relative;
    padding-left: 35px;
    margin-bottom: 12px;
    cursor: pointer;
    font-size: 22px;
    user-select: none; 
    
    & input {
        display: none;
    }
    
    &:hover input ~ ${Checkmark} {
        background-color: #ccc;
    }
    
    & input:checked ~ ${Checkmark} {
        background-color: #007bff;
        border-color: #007bff;
    }
    
    & input:checked ~ ${Checkmark}:after {
        display: block;
    }

    & ${Checkmark}:after {
        left: 9px;
        top: 6px;
        width: 5px;
        height: 10px;
        border: solid white;
        border-width: 0 2px 2px 0;
        transform: rotate(45deg);
    }
`;

const Checkbox = ({ checked, onChange }) =>
    <CheckboxContainer>
        <input type="checkbox" onChange={onChange} checked={checked} />
        <Checkmark />
    </CheckboxContainer>;

Checkbox.propTypes = {
    checked: PropTypes.bool,
    onChange: PropTypes.func,
};

export default Checkbox;
