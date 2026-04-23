export const extractYearFromDate = (date: string) => {
  const year = date.split("-")[0];
  return year.length === 4 ? year : "UNKNOWN";
};
