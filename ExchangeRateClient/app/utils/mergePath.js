import { __, assocPath, compose, curry, merge, path } from 'ramda';

export default curry((pathTo, newValues, obj) => compose(
    assocPath(pathTo, __, obj),
    merge(__, newValues),
    path(pathTo),
)(obj));
