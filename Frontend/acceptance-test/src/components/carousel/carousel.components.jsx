import React, { useState, useEffect } from "react";
import { Gallery, GalleryImage } from "react-gesture-gallery";

//Data
import { IMAGES } from "../../data/images.js";

//Styles
import "./carousel.styles.scss";

const Carousel = () => {
  const [index, setIndex] = useState(0);

  useEffect(() => {
    const interval = setInterval(() => {
      if (index === IMAGES.length - 1) {
        setIndex(0);
      } else {
        setIndex(index + 1);
      }
    }, 2500);
    return () => clearInterval(interval);
  }, [index]);

  return (
    <>
      <Gallery
        className="carousel"
        index={index}
        onRequestChange={i => {
          setIndex(i);
        }}
      >
        {IMAGES.map(img => (
          <GalleryImage
            className="carousel__image"
            src={img.src}
            key={img.src}
            objectFit="cover"
          />
        ))}
      </Gallery>
    </>
  );
};

export default Carousel;
