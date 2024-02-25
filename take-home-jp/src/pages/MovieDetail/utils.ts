export const formatFullYearDate = (dateString?: string) => {
  return dateString ? new Date(dateString).getFullYear() : '';
};

export const displayMovieRunTime = (runTime?: number): string => {
  if (!runTime) {
    return '';
  }
  const minutes = runTime % 60;
  const hours = Math.floor(runTime / 60);
  return `${hours}h ${minutes}`;
};
