/**
 * Formats a date string into a human-readable format based on user locale.
 *
 * @param dateString - The date string to be formatted (e.g., "2024-05-02").
 * @returns A formatted date string (e.g., "05/02/2024" or "May 2nd, 2024" depending on locale).
 *
 * @throws Throws an error if the input date string cannot be parsed into a valid Date object.
 */
export default function formatDate(dateString: string): string {
  const date = new Date(dateString);

  const options: Intl.DateTimeFormatOptions = {
    year: 'numeric',
    month: '2-digit',
    day: 'numeric'
  };

  return date.toLocaleDateString(navigator.language, options);
}
