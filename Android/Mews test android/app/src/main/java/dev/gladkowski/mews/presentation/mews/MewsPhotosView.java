package dev.gladkowski.mews.presentation.mews;

import com.arellomobile.mvp.viewstate.strategy.AddToEndStrategy;
import com.arellomobile.mvp.viewstate.strategy.StateStrategyType;

import java.util.List;

import dev.gladkowski.mews.entity.mews.presentation.BaseMewsPhotoItem;
import dev.gladkowski.mews.presentation.common.fragment.BaseFragmentView;


/**
 * Interface for MewsPhotosFragment
 */
public interface MewsPhotosView extends BaseFragmentView {

    /**
     * Show the high res image
     */
    @StateStrategyType(AddToEndStrategy.class)
    void showImage(String imageUrl);

    /**
     * Show the list of items
     */
    @StateStrategyType(AddToEndStrategy.class)
    void showList(List<BaseMewsPhotoItem> items);

    /**
     * Add more items to the list and show it
     */
    @StateStrategyType(AddToEndStrategy.class)
    void addMoreItems(List<BaseMewsPhotoItem> items);

    /**
     * Clear list of items
     */
    void clearList();

    /**
     * Show loading at the end of the list
     */
    void onShowPageLoading();

    /**
     * Show loading at the end of the list
     */
    void onHidePageLoading();
}
