export default function formatCurrency(value: number, desiredCurrency = 'USD'): string {
  const formatter = new Intl.NumberFormat(navigator.language, {
    style: 'currency',
    currency: desiredCurrency
  });

  return formatter.format(value);
}
