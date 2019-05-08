package dev.gladkowski.mews.utils.imageloader;

import android.widget.ImageView;

import dev.gladkowski.mews.utils.imageloader.glide.ImageLoaderCallback;


/**
 * Load image into ImageView
 */
public interface ImageLoader {

    /**
     * Set image with callback
     */
    void setImageWithCallback(ImageView imageView, String url, ImageLoaderCallback callback);

    /**
     * Set image with center crop
     */
    void setImageCenterCrop(ImageView imageView, String url);

    /**
     * Set image that fits center
     */
    void setImageFitCenter(ImageView imageView, String url);

    /**
     * Clear image
     */
    void clearImage(ImageView imageView);
}
