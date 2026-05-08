import { getImageURL } from '../../utils';
import { StyledCardMovieImage } from './Image.styles';
import noImageAvailable from '../../assets/no_image_available.jpg';
import { useIsMobile } from '../../hooks/useIsMobile';
import type { ImageParams } from '../types';

export const Image = ({
  imageURL,
  title,
  fileSize,
  mobileImageURL,
  rounded = false,
}: ImageParams) => {
  const isMobile = useIsMobile();
  const imagePath = isMobile && mobileImageURL ? mobileImageURL : imageURL;

  return (
    <StyledCardMovieImage
      src={imagePath ? getImageURL(imagePath, fileSize) : noImageAvailable}
      alt={title}
      loading="lazy"
      $rounded={rounded}
    />
  );
};
