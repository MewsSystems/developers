import { SortingOrder } from '../models/SortingOrder';

/**
 * Compares two objects for descending order based on a specified property.
 *
 * @typeparam T The type of the objects to compare.
 *
 * @param {T} a The first object to compare.
 * @param {T} b The second object to compare.
 * @param {keyof T} orderBy The key (property name) to use for sorting.
 *
 * @returns {number} A number indicating the comparison result:
 *   - `-1`: If `b[orderBy]` is less than `a[orderBy]`.
 *   - `1`: If `b[orderBy]` is greater than `a[orderBy]`.
 *   - `0`: If `b[orderBy]` is equal to `a[orderBy]`.
 */
function descendingComparator<T>(a: T, b: T, orderBy: keyof T) {
  if (b[orderBy] < a[orderBy]) {
    return -1;
  }
  if (b[orderBy] > a[orderBy]) {
    return 1;
  }
  return 0;
}

/**
 * Creates a sorting function based on the provided order and sort key.
 *
 * @typeparam Key Extends `keyof any` to ensure the key is a property of the object type.
 *
 * @param {SortingOrder} order The {@link SortingOrder | sorting order} (`'asc'` for ascending, `'desc'` for descending).
 * @param {Key} orderBy The key (property name) to use for sorting.
 *
 * @returns {(a: { [key in Key]: number | string }, b: { [key in Key]: number | string }) => number}
 *  A sorting function that takes two objects as arguments and returns a number indicating the comparison result.
 */
export function getComparator<Key extends keyof any>(
  order: SortingOrder,
  orderBy: Key
): (a: { [key in Key]: number | string }, b: { [key in Key]: number | string }) => number {
  return order === 'desc'
    ? (a, b) => descendingComparator(a, b, orderBy)
    : (a, b) => -descendingComparator(a, b, orderBy);
}
