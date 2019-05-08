package dev.gladkowski.mews.utils.imageloader.glide;

import android.content.Context;
import android.graphics.drawable.Drawable;
import android.support.annotation.Nullable;
import android.support.v4.widget.CircularProgressDrawable;
import android.widget.ImageView;

import com.bumptech.glide.load.DataSource;
import com.bumptech.glide.load.engine.DiskCacheStrategy;
import com.bumptech.glide.load.engine.GlideException;
import com.bumptech.glide.request.RequestListener;
import com.bumptech.glide.request.target.Target;

import javax.inject.Inject;

import dev.gladkowski.mews.R;
import dev.gladkowski.mews.utils.imageloader.ImageLoader;


/**
 * Implementation of ImageLoader for Glide Library
 */
public class GlideImageLoaderImpl implements ImageLoader {

    private Context context;

    @Inject
    public GlideImageLoaderImpl(Context context) {
        this.context = context;
    }

    @Override
    public void setImageWithCallback(ImageView imageView, String url, ImageLoaderCallback callback) {
        GlideApp.with(imageView.getContext())
                .load(url)
                .diskCacheStrategy(DiskCacheStrategy.DATA)
                .skipMemoryCache(true)
                .error(R.drawable.ic_error)
                .listener(new RequestListener<Drawable>() {
                    @Override
                    public boolean onLoadFailed(@Nullable GlideException e, Object model, Target<Drawable> target, boolean isFirstResource) {
                        callback.onImageLoadFailed();
                        return false;
                    }

                    @Override
                    public boolean onResourceReady(Drawable resource, Object model, Target<Drawable> target, DataSource dataSource, boolean isFirstResource) {
                        callback.onImageLoadSuccess();
                        return false;
                    }
                })
                .into(imageView);
    }

    @Override
    public void setImageCenterCrop(ImageView imageView, String url) {
        CircularProgressDrawable circularProgressDrawable = new CircularProgressDrawable(context);
        circularProgressDrawable.setStrokeWidth(5f);
        circularProgressDrawable.setCenterRadius(30f);
        circularProgressDrawable.start();


        GlideApp.with(imageView.getContext())
                .load(url)
                .diskCacheStrategy(DiskCacheStrategy.DATA)
                .placeholder(circularProgressDrawable)
                .skipMemoryCache(true)
                .fitCenter()
                .centerCrop()
                .error(R.drawable.ic_error)
                .into(imageView);
    }

    @Override
    public void setImageFitCenter(ImageView imageView, String url) {
        GlideApp.with(imageView.getContext())
                .load(url)
                .diskCacheStrategy(DiskCacheStrategy.DATA)
                .skipMemoryCache(true)
                .fitCenter()
                .error(R.drawable.ic_error)
                .into(imageView);
    }

    @Override
    public void clearImage(ImageView imageView) {
        GlideApp.with(imageView).clear(imageView);
    }
}
