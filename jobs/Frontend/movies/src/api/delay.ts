function delay(milliseconds: number) {
  return new Promise((resolve) =>
    setTimeout(() => {
      resolve(milliseconds);
    }, milliseconds)
  );
}

export default delay;
