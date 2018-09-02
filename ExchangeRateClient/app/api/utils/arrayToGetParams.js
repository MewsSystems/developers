import { addIndex, concat, reduceRight } from 'ramda';

const reduceIndexed = addIndex(reduceRight);

const addToGetParam = (paramName) => (id, accumulator, index) =>
    concat(accumulator, `${paramName}[${index}]=${id}&`);

export default (paramName) => reduceIndexed(addToGetParam(paramName), '');
