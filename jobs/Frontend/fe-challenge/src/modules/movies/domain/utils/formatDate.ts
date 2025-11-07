const defaultOptions: Intl.DateTimeFormatOptions = {
  month: 'short',
  day: 'numeric',
  year: 'numeric',
};

export const formatDate = (
  inputDate: string,
  options: Intl.DateTimeFormatOptions = defaultOptions,
): string => {
  const date = new Date(inputDate);
  const formattedDate = date.toLocaleDateString('en-US', options);

  return formattedDate;
};
