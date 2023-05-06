export const formatCurrency = (value?: number | string, currency: string = "USD") => {
  if (!value) return null;
  const number = Number(value);
  return new Intl.NumberFormat("en-US", {
    style: "currency",
    currency,
    maximumFractionDigits: 0,
  }).format(number);
};

export const formatNumber = (value?: number) => {
  if (!value) return null;
  const number = Number(value);
  return new Intl.NumberFormat("en-US", {
    maximumFractionDigits: 0,
  }).format(number);
};
