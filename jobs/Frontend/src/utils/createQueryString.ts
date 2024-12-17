const createQueryString = (queries: {
  [key: string]: string | number | null;
}): string => {
  return Object.entries(queries).reduce((acc, [key, value]) => {
    if (value === undefined || value === null || value === "") return acc;
    return acc.includes("?") ? `${acc}&${key}=${value}` : `?${key}=${value}`;
  }, "");
};

export default createQueryString;
