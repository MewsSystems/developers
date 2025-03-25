/*
  Animation variants for container elements
 */
export const containerVariants = {
  hidden: { opacity: 0 },
  visible: {
    opacity: 1,
    transition: { staggerChildren: 0.05 },
  },
};

export const fadeIn = {
  hidden: { opacity: 0, y: 20 },
  visible: (delay = 0) => ({
    opacity: 1,
    y: 0,
    transition: { duration: 0.5, delay },
  }),
};

export const posterVariants = {
  hidden: { opacity: 0, scale: 0.9, rotateY: -30 },
  visible: {
    opacity: 1,
    scale: 1,
    rotateY: 0,
    transition: { duration: 0.6, type: "spring", stiffness: 100 },
  },
};

export const castVariant = {
  hidden: { opacity: 0, x: -20 },
  visible: (delay = 0) => ({
    opacity: 1,
    x: 0,
    transition: { duration: 0.3, delay },
  }),
  hover: {
    scale: 1.03,
    backgroundColor: "rgba(149, 0, 255, 0.2)",
  },
};
