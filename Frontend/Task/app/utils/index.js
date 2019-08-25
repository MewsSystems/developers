import R from 'ramda';

export const trunc = R.when(
  R.propSatisfies(R.gt(R.__, 20), 'length'),
  R.pipe(
    R.take(20),
    R.append('...'),
    R.join('')
  )
);

export const transformPairs = pairs => {
  return R.map(pair => {
    return {
      codeFrom: pair[0].code,
      codeTo: pair[1].code
    };
  }, pairs);
};

export const transformRates = rates => {
  return R.map(num => {
    return {
      rate: num
    };
  }, rates);
};

export const transformTrends = trends => {
  return R.map(trend => {
    return {
      trend
    };
  }, trends);
};

const rateComparator = newRate =>
  R.cond([
    [R.gt(newRate), R.always('growing')],
    [R.lt(newRate), R.always('declining')],
    [R.T, R.always('stagnating')]
  ]);

const compareRates = (oldRate, newRate) => {
  return rateComparator(newRate)(oldRate);
};

export const calculateTrends = (oldRates, newRates) => {
  if (R.or(R.isEmpty(oldRates), R.isEmpty(newRates))) return {};
  return R.mergeWith(compareRates, oldRates, newRates);
};

const byDesc = key =>
  R.descend(
    R.compose(
      R.prop(key),
      R.last
    )
  );

const byAsc = key =>
  R.ascend(
    R.compose(
      R.prop(key),
      R.last
    )
  );

export const sortData = (key, order) => {
  return order === 'desc' ? R.sort(byDesc(key)) : R.sort(byAsc(key));
};
