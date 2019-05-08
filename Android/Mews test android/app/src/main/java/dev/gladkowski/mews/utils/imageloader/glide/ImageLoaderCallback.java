package dev.gladkowski.mews.utils.imageloader.glide;

/**
 * Callback interface for ImageLoader
 */
public interface ImageLoaderCallback {

    /**
     * Image has loaded successfully
     */
    void onImageLoadSuccess();

    /**
     * Image loading has failed
     */
    void onImageLoadFailed();
}
