export const formatDate = (inputDate: string): string => {
  const date = new Date(inputDate);

  const options: Intl.DateTimeFormatOptions = {
    month: 'short',
    day: 'numeric',
    year: 'numeric',
  };

  const formattedDate = date.toLocaleDateString('en-US', options);

  return formattedDate;
};
