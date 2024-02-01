export const removeConsecutiveDuplicates = (array: number[]): number[] => {
    return array.filter((item, index) => item !== array[index - 1]);
};
