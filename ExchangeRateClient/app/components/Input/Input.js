import React from 'react';
import { always, cond, equals, T } from 'ramda';
import { Input as BSInput } from 'reactstrap';
import { Checkbox } from './inputTypes';

// eslint-disable-next-line ramda/cond-simplification
const prepareInputComponent = (props) => cond([
    [equals('checkbox'), always(<Checkbox {...props} />)],
    [T, always(<BSInput {...props} />)],
]);

const Input = (props) => prepareInputComponent(props)(props.type);

export default Input;
