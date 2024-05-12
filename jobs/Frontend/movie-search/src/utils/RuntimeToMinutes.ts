export function runtimeToHoursMinutes(runtimeMinutes: number) {
  const hours = Math.floor(runtimeMinutes / 60);
  const minutes = runtimeMinutes % 60;
  return `${hours}H ${minutes}M`;
}
