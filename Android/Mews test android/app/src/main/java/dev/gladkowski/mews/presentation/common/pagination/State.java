package dev.gladkowski.mews.presentation.common.pagination;

import java.util.List;

import dev.gladkowski.mews.entity.mews.presentation.BaseMewsPhotoItem;


/**
 * Paginator state interface
 */
public interface State {

    /**
     * Refresh list
     */
    void refresh();

    /**
     * Load new page
     */
    void loadNewPage();

    /**
     * Release paginator
     */
    void release();

    /**
     * New list items loaded
     */
    void newData(List<BaseMewsPhotoItem> list);

    /**
     * Error happened
     */
    void error(Throwable throwable);
}
