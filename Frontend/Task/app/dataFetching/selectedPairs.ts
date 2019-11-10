import * as t from 'io-ts';

const storageItemKey = 'selectedPairs';

export function storeSelectedPairs(
  selectedPairIds: ReadonlyArray<string>,
): ReadonlyArray<string> {
  // eslint-disable-next-line @typescript-eslint/ban-ts-ignore
  // @ts-ignore
  global.localStorage.setItem(storageItemKey, JSON.stringify(selectedPairIds));
  return selectedPairIds;
}

const pairIdsCodec = t.readonlyArray(t.string);

export function loadSelectedPairs(): ReadonlyArray<string> {
  try {
    // eslint-disable-next-line @typescript-eslint/ban-ts-ignore
    // @ts-ignore
    const storedValue = global.localStorage.getItem(storageItemKey);
    const storedValueDecoded = pairIdsCodec.decode(JSON.parse(storedValue));
    if (storedValueDecoded._tag === 'Right') {
      return storedValueDecoded.right;
    }
  } catch (error) {}
  return [];
}
