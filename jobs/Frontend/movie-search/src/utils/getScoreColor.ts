export const getScoreColor = (score: number) => {
  if (score > 7) {
    return "green";
  } else if (score > 5) {
    return "orange";
  } else {
    return "red";
  }
};
