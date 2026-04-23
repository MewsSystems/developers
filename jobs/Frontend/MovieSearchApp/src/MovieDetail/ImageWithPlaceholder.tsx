import React, { useState } from "react";

export type ImageProps = {
  src: string;
  alt: string;
  height: number;
  width: number;
  className: string;
  loadingPlaceholder: JSX.Element;
  errorPlaceholder: JSX.Element;
};

export const ImageWithPlaceholder: React.FC<ImageProps> = ({
  src,
  alt,
  className,
  height,
  width,
  loadingPlaceholder,
  errorPlaceholder,
}) => {
  const [imageLoaded, setImageLoaded] = useState(false);
  const [error, setError] = useState(false);

  const image = new Image();
  image.src = src;
  image.onload = () => setImageLoaded(true);
  image.onerror = () => setError(true);

  if (imageLoaded) {
    return <img height={height} width={width} alt={alt} src={image.src} className={className} />;
  }

  if (error) {
    return errorPlaceholder;
  }

  return loadingPlaceholder;
};
