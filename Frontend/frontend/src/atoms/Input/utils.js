/**
 * Get Input Size
 */
export const getSizeInput = ({ size, }) => {
  const sizes = {
    sm: `
      font-size: 0.85rem;
    `,
    md: `
      font-size: 1rem;
      `,
    lg: `
      font-size: 1.15rem;
    `,
  };

  if (Object.prototype.hasOwnProperty.call(sizes, size)) {
    return sizes[size];
  }
  return sizes.md;
};
