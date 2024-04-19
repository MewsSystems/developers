import { FallbackImgImg, FallbackImgPlaceholder } from './FallbackImg.styled.tsx';
import { IconType } from 'react-icons';
import { ImgHTMLAttributes } from 'react';

interface FallbackImgProps {
    alt: string;
    src: string;
    imgProps?: ImgHTMLAttributes<HTMLImageElement>;
    cssAspectRatio: string;
    placeholderIcon: IconType;
    placeholderLabel: string;
}

export function FallbackImg({ alt, src, imgProps, cssAspectRatio, placeholderIcon, placeholderLabel }: FallbackImgProps) {
    if (src) {
        return <FallbackImgImg alt={alt} src={src} {...imgProps} style={{ aspectRatio: cssAspectRatio }} />
    }

    return (
        <FallbackImgPlaceholder
            aria-label={placeholderLabel}
            role='img'
            style={{ aspectRatio: cssAspectRatio }}
        >
            {placeholderIcon({ 'aria-hidden': true })}
        </FallbackImgPlaceholder>
    );
}