/**
 * Formats a number as currency based on user locale and desired currency.
 *
 * @param value - The numeric value to be formatted as currency.
 * @param desiredCurrency (optional) - The desired currency code (e.g., 'USD', 'EUR', 'JPY'). Defaults to 'USD'.
 * @returns A formatted currency string (e.g., "$1,234.56" or "€1.234,56" depending on locale and currency).
 */
export default function formatCurrency(value: number, desiredCurrency = 'USD'): string {
  const formatter = new Intl.NumberFormat(navigator.language, {
    style: 'currency',
    currency: desiredCurrency
  });

  return formatter.format(value);
}
