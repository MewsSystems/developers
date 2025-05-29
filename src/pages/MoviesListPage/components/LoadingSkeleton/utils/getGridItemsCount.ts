export const getGridItemsCount = () => {
  let columnsPerRow = 1;

  // Adjust based on viewport width
  if (typeof window !== 'undefined') {
    const width = window.innerWidth;
    if (width > 1400) columnsPerRow = 5;
    else if (width > 1100) columnsPerRow = 4;
    else if (width > 800) columnsPerRow = 3;
    else if (width > 500) columnsPerRow = 2;
    else columnsPerRow = 1;
  }

  return columnsPerRow * 2;
};
