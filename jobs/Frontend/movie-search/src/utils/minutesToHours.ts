export const minutesToHoursMinutes = (minutes: number) => {
  var hours = Math.floor(minutes / 60);
  var remainingMinutes = minutes % 60;
  return [hours, remainingMinutes];
};
