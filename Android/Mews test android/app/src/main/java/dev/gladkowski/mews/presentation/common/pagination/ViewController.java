package dev.gladkowski.mews.presentation.common.pagination;

import java.util.List;

/**
 * Interface for controlling the view
 */
public interface ViewController<T> {

    /**
     * Show empty list view
     */
    void showEmptyView(boolean show);

    /**
     * Show list
     */
    void showList(boolean show, List<T> list);

    /**
     * Show refresh progress at the top of screen
     */
    void showRefreshProgress(boolean show);

    /**
     * Show page progress at the end of the list
     */
    void showPageProgress(boolean show);

    /**
     * Show error
     */
    void showError(Throwable throwable);
}
