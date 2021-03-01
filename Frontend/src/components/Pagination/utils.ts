import { range } from '../../utils';

export const generatePageNumSequence = (
  current: number,
  first: number,
  last: number,
  sequenceLength = 2,
  gapSeparator = '...'
) => {
  const pageNumSequence = new Set([
    ...range(first, first + sequenceLength),
    ...range(
      Math.max(current - sequenceLength / 2, first),
      Math.min(current + sequenceLength / 2, last)
    ),
    ...range(last - sequenceLength, last),
  ]);

  const sortedSequence = Array.from(pageNumSequence).sort((a, b) => a - b);

  return sortedSequence.reduce((res, value, index, arr) => {
    res.push({ id: `${value}`, label: `${value}`, value });

    // check if there's a gap in the sequence
    const nextValue = arr[index + 1];
    if (nextValue && nextValue - value > 1) {
      res.push({
        id: `${value}.${nextValue}`,
        label: gapSeparator,
        value: null,
      });
    }

    return res;
  }, [] as Array<{ id: string; label: string; value: number | null }>);
};
