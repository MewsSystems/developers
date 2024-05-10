export const dateFormatter = (date: string) => {
  const newDate = new Date(date);
  return newDate.toLocaleString(undefined, {
    year: "numeric",
    month: "long",
    day: "2-digit",
  });
};
