import { append, compose, join, take, when } from 'ramda';
import { stringIsLongerThan } from './index';

export default (maxLength) => when(
    stringIsLongerThan(maxLength),
    compose(join(''), append('...'), take(maxLength)),
);
