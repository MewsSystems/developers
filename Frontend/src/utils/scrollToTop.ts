const scrollToTop = () => {
  const position = document.documentElement.scrollTop || document.body.scrollTop;
  if (position > 0) {
    window.requestAnimationFrame(scrollToTop);
    window.scrollTo(0, position - position / 5);
  }
};

export default scrollToTop;
