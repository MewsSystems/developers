import { compose, map, prop } from 'ramda';
import { alwaysEmptyArray } from 'ramda-extension';

export default compose(map(alwaysEmptyArray), prop('currencyPairs'));
