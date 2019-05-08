package dev.gladkowski.mews.presentation.common.pagination;

/**
 * Interface for pagination
 */
public interface Paginator {

    /**
     * Refresh the list
     */
    void refresh();

    /**
     * Load new page
     */
    void loadNewPage();

    /**
     * Stop working with paginator
     */
    void release();
}
