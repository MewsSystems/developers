
import styled from 'styled-components';
import fallbackImage from './fallback-image.png';
import { ReactNode } from 'react';

interface ImgPropsStyleTypes {
  $objectFit?: ImgProps['objectFit'];
}

export const StyledImg = styled.img<ImgPropsStyleTypes>`
  width: 100%;
  height: 100%;
  object-fit: ${(props: ImgPropsStyleTypes) => props?.$objectFit ?? 'cover'};
`;

interface ImgProps {
  src: string;
  loading?: 'eager' | 'lazy';
  alt: string;
  ariaLabel?: string;
  objectFit?: 'contain' | 'cover' | 'fill';
}

export const Img = (props: ImgProps): ReactNode => {
  const { src, ariaLabel, alt, loading = 'lazy', objectFit } = props;

  return (
    <StyledImg
      src={src}
      aria-label={ariaLabel ?? alt}
      alt={alt}
      loading={loading}
      $objectFit={objectFit}
      onError={(e: React.SyntheticEvent<HTMLImageElement, Event>) => {
        e.currentTarget.onerror = null;
        e.currentTarget.src = fallbackImage;
        e.currentTarget.style.transform = 'scale(1)';
      }}
    />
  );
};
