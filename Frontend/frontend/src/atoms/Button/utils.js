export const getSizeBtn = ({ size, }) => {
  const sizes = {
    xs: `
      font-size: 0.6rem;
    `,
    sm: `
      font-size: 0.75rem;
    `,
    md: `
      font-size: 0.875rem;
      `,
    lg: `
      font-size: 1rem;
    `,
  };

  if (Object.prototype.hasOwnProperty.call(sizes, size)) {
    return sizes[size];
  }
  return sizes.md;
};
