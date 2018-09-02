import React from 'react';
import PropTypes from 'prop-types';
import { compose, values } from 'ramda';
import { mapIndexed } from 'ramda-adjunct';
import ErrorMessage from './ErrorMessage';

// eslint-disable-next-line react/display-name
const renderErrorMessage = (timeout) => (errorMessage, index) =>
    <ErrorMessage
        key={`error${index}`}
        errorMessage={errorMessage}
        timeout={timeout}
    />;

const ErrorMessages = ({ errorMessages, timeout }) =>
    <React.Fragment>
        {compose(mapIndexed(renderErrorMessage(timeout)), values)(errorMessages)}
    </React.Fragment>;

ErrorMessages.propTypes = {
    errorMessages: PropTypes.any,
    timeout: PropTypes.number,
};

export default ErrorMessages;
