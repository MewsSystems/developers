export const getCurrentTime = (): number => new Date().getTime();

export const splitDate = (date: string): string[] =>
  date ? date.split('-') : [];

export const range = (start: number, end: number) =>
  Array.from({ length: end - start + 1 }, (_, i) => i + start);
