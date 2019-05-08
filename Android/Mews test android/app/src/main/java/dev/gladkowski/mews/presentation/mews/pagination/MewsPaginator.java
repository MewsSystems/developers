package dev.gladkowski.mews.presentation.mews.pagination;


import dev.gladkowski.mews.presentation.common.pagination.Paginator;

public interface MewsPaginator extends Paginator {

    /**
     * Returns if the current page is first
     */
    boolean isFirstPage();
}
