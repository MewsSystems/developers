import { __, gt, propSatisfies } from 'ramda';

export default (maxLength) => propSatisfies(gt(__, maxLength), 'length');
