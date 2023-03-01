import React, { ReactNode } from "react";
import { GalleryLayout, InnerGalleryLayout } from "./MoviePreviewGallery.styled";

interface MoviePreviewGalleryProps {
  children: ReactNode;
}

export const MoviePreviewGallery = (props: MoviePreviewGalleryProps) => {
  return (
    <GalleryLayout>
      <InnerGalleryLayout>
        {props.children}
      </InnerGalleryLayout>
    </GalleryLayout>
  )
};
