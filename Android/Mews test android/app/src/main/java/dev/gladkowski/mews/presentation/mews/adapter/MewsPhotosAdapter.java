package dev.gladkowski.mews.presentation.mews.adapter;

import com.hannesdorfmann.adapterdelegates3.ListDelegationAdapter;

import java.util.ArrayList;
import java.util.List;

import dev.gladkowski.mews.entity.common.presentation.BaseItem;
import dev.gladkowski.mews.entity.common.presentation.ProgressItem;
import dev.gladkowski.mews.entity.mews.presentation.BaseMewsPhotoItem;
import dev.gladkowski.mews.presentation.mews.adapter.callback.MewsPhotoItemCallback;
import dev.gladkowski.mews.utils.imageloader.ImageLoader;


/**
 * Adapter for list of mews photos
 */
public class MewsPhotosAdapter extends ListDelegationAdapter<List<BaseItem>> {

    private ProgressItem progressItem;

    public MewsPhotosAdapter(MewsPhotoItemCallback mewsPhotoItemCallback, ImageLoader imageLoader) {
        delegatesManager.addDelegate(new MewsPhotosAdapterDelegate(mewsPhotoItemCallback, imageLoader));
        delegatesManager.addDelegate(new ProgressItemAdapterDelegate());

        progressItem = new ProgressItem();

        setItems(new ArrayList<>());
    }

    /**
     * Put items to the list
     */
    public void addItems(List<BaseMewsPhotoItem> baseItems) {
        items.clear();
        items.addAll(baseItems);
        this.notifyDataSetChanged();
    }

    /**
     * Add more items
     */
    public void addMoreItems(List<BaseMewsPhotoItem> baseItems) {
        items.addAll(baseItems);
        notifyItemChanged(getItemCount());
    }

    /**
     * Clear list
     */
    public void clearList() {
        items.clear();
        notifyDataSetChanged();
    }

    /**
     * Show loading spinner in the end of list
     */
    public void showPageLoading() {
        if (!items.contains(progressItem)) {
            items.add(progressItem);
            notifyItemChanged(items.size() - 1);
        }
    }

    /**
     * Hide loading spinner in the end of list
     */
    public void hidePageLoading() {
        if (items.contains(progressItem)) {
            items.remove(progressItem);
            notifyItemRemoved(items.size());
        }
    }

    /**
     * Get item object by it's position in list
     */
    public BaseItem getItemByPosition(int position) {
        return items.get(position);
    }
}
