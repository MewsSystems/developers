import { compose, defaultTo, prop } from 'ramda';

export const getErrorMessages = compose(defaultTo([]), prop('errorMessages'));
