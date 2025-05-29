export const getGridItemsCount = () => {
  // Default to mobile size
  let count = 2;

  // Adjust based on viewport width
  if (typeof window !== 'undefined') {
    const width = window.innerWidth;
    if (width > 1400)
      count = 10; // 2 rows of 5
    else if (width > 1100)
      count = 8; // 2 rows of 4
    else if (width > 800)
      count = 6; // 2 rows of 3
    else if (width > 500)
      count = 4; // 2 rows of 2
    else count = 2; // 2 rows of 1
  }

  return count;
};
