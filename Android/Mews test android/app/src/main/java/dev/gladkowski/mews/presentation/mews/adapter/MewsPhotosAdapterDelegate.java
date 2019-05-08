package dev.gladkowski.mews.presentation.mews.adapter;

import android.animation.ValueAnimator;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.constraint.ConstraintLayout;
import android.support.v7.widget.CardView;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.ViewTreeObserver;
import android.view.animation.AccelerateDecelerateInterpolator;
import android.view.animation.AlphaAnimation;
import android.view.animation.Animation;
import android.widget.ImageView;
import android.widget.TextView;

import com.hannesdorfmann.adapterdelegates3.AdapterDelegate;

import java.util.List;

import butterknife.BindView;
import butterknife.ButterKnife;
import dev.gladkowski.mews.R;
import dev.gladkowski.mews.entity.common.presentation.BaseItem;
import dev.gladkowski.mews.entity.mews.presentation.MewsPhotoItem;
import dev.gladkowski.mews.presentation.common.constants.AppConstants;
import dev.gladkowski.mews.presentation.mews.adapter.callback.MewsPhotoItemCallback;
import dev.gladkowski.mews.utils.imageloader.ImageLoader;

/**
 * Delegate with Mews Photo item
 */
public class MewsPhotosAdapterDelegate extends AdapterDelegate<List<BaseItem>> {

    @NonNull
    private MewsPhotoItemCallback mewsPhotoItemCallback;
    @NonNull
    private ImageLoader imageLoader;

    MewsPhotosAdapterDelegate(@NonNull MewsPhotoItemCallback mewsPhotoItemCallback,
                              @NonNull ImageLoader imageLoader) {
        this.mewsPhotoItemCallback = mewsPhotoItemCallback;
        this.imageLoader = imageLoader;
    }

    @Override
    public boolean isForViewType(@NonNull List items, int position) {
        return items.get(position) instanceof MewsPhotoItem;
    }

    @NonNull
    @Override
    public RecyclerView.ViewHolder onCreateViewHolder(ViewGroup parent) {
        View item = LayoutInflater
                .from(parent.getContext())
                .inflate(R.layout.item_mews_photo, parent, false);
        return new ViewHolder(item);
    }

    @Override
    public void onBindViewHolder(@NonNull List<BaseItem> items,
                                 int position,
                                 @NonNull RecyclerView.ViewHolder holder,
                                 @Nullable List<Object> payloads) {

        MewsPhotosAdapterDelegate.ViewHolder viewHolder = (MewsPhotosAdapterDelegate.ViewHolder) holder;
        MewsPhotoItem viewModel = (MewsPhotoItem) items.get(position);
        viewHolder.setItem(viewModel);
    }

    class ViewHolder extends RecyclerView.ViewHolder {

        private int originalHeight = 0;

        @BindView(R.id.card_view)
        CardView cardView;
        @BindView(R.id.container)
        ConstraintLayout container;
        @BindView(R.id.image_thumbnail)
        ImageView imageView;
        @BindView(R.id.layout_additional_info)
        ConstraintLayout additionalInfo;
        @BindView(R.id.text_title)
        TextView textTitle;
        @BindView(R.id.text_photo_id)
        TextView textPhotoId;
        @BindView(R.id.text_album_id)
        TextView textAlbumId;

        ViewHolder(View itemView) {
            super(itemView);
            ButterKnife.bind(this, itemView);
        }

        private void setItem(MewsPhotoItem item) {
            imageLoader.setImageCenterCrop(imageView, item.getThumbnailUrl());
            textTitle.setText(item.getTitle());
            textPhotoId.setText(String.valueOf(item.getId()));
            textAlbumId.setText(String.valueOf(item.getAlbumId()));

            cardView.getViewTreeObserver().addOnPreDrawListener(new ViewTreeObserver.OnPreDrawListener() {
                @Override
                public boolean onPreDraw() {
                    cardView.getViewTreeObserver().removeOnPreDrawListener(this);
                    ViewGroup.LayoutParams layoutParams = cardView.getLayoutParams();
                    if (originalHeight == 0) {
                        originalHeight = cardView.getHeight();
                    }
                    if (!item.isExpanded()) {
                        showView(additionalInfo, false);
                        layoutParams.height = originalHeight;
                    } else {
                        showView(additionalInfo, true);
                        layoutParams.height = originalHeight * 2;
                    }
                    cardView.setLayoutParams(layoutParams);
                    return true;
                }
            });

            container.setOnClickListener(view -> {
                        resizeItem(cardView, item);
                        mewsPhotoItemCallback.onItemClick(item.getId(), item.getUrl());
                    }
            );
        }

        private void resizeItem(final View view, MewsPhotoItem item) {
            if (originalHeight == 0) {
                originalHeight = view.getHeight();
            }
            ValueAnimator valueAnimator;
            if (!item.isExpanded()) {
                item.setExpanded(true);
                showView(additionalInfo, true);
                valueAnimator = ValueAnimator.ofInt(originalHeight, originalHeight * 2);
            } else {
                item.setExpanded(false);
                valueAnimator = ValueAnimator.ofInt(originalHeight + originalHeight, originalHeight);
                Animation a = new AlphaAnimation(1.00f, 0.00f); // Fadeout animation
                a.setDuration(AppConstants.EXPAND_ITEM_ANIMATION_DURATION);
                a.setAnimationListener(new Animation.AnimationListener() {
                    @Override
                    public void onAnimationStart(Animation animation) {

                    }

                    @Override
                    public void onAnimationEnd(Animation animation) {
                        showView(additionalInfo, false);
                    }

                    @Override
                    public void onAnimationRepeat(Animation animation) {

                    }
                });
                additionalInfo.startAnimation(a);
            }
            valueAnimator.setDuration(AppConstants.EXPAND_ITEM_ANIMATION_DURATION);
            valueAnimator.setInterpolator(new AccelerateDecelerateInterpolator());
            valueAnimator.addUpdateListener(animation -> {
                view.getLayoutParams().height = (Integer) animation.getAnimatedValue();
                view.requestLayout();
            });
            valueAnimator.start();
        }
    }

    private void showView(View view, boolean show) {
        if (show) {
            view.setVisibility(View.VISIBLE);
            view.setEnabled(true);
        } else {
            view.setVisibility(View.GONE);
            view.setEnabled(false);
        }
    }
}
