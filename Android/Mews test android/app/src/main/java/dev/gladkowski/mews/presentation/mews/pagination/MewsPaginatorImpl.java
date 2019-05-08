package dev.gladkowski.mews.presentation.mews.pagination;

import java.util.Collections;
import java.util.List;

import dev.gladkowski.mews.domain.mews.MewsPhotoInteractor;
import dev.gladkowski.mews.entity.mews.presentation.BaseMewsPhotoItem;
import dev.gladkowski.mews.presentation.common.constants.AppConstants;
import dev.gladkowski.mews.presentation.common.pagination.State;
import dev.gladkowski.mews.presentation.common.pagination.ViewController;
import dev.gladkowski.mews.presentation.mews.converter.MewsPhotoItemConverter;
import io.reactivex.disposables.Disposable;

public class MewsPaginatorImpl implements MewsPaginator {

    private MewsPhotoInteractor interactor;
    private ViewController<BaseMewsPhotoItem> viewController;
    private MewsPhotoItemConverter converter;

    private int currentLastItem;
    private int startItem = 0;

    private State currentState = new INITIAL_STATE();
    private List<BaseMewsPhotoItem> currentData = Collections.emptyList();
    private Disposable disposable;

    public MewsPaginatorImpl(MewsPhotoInteractor interactor,
                             ViewController<BaseMewsPhotoItem> viewController,
                             MewsPhotoItemConverter converter) {
        this.interactor = interactor;
        this.viewController = viewController;
        this.converter = converter;
    }

    @Override
    public boolean isFirstPage() {
        return currentLastItem == startItem + AppConstants.PHOTO_ITEMS_PAGE_LIMIT;
    }

    @Override
    public void refresh() {
        currentState.refresh();
    }

    @Override
    public void loadNewPage() {
        currentState.loadNewPage();
    }

    @Override
    public void release() {
        currentState.release();
    }

    private void loadPage(int page) {
        unsubscribe();

        disposable = interactor.getMewsPhotosByPage(page, AppConstants.PHOTO_ITEMS_PAGE_LIMIT)
                .map(converter)
                .subscribe(items -> currentState.newData(items),
                        throwable -> currentState.error(throwable));
    }

    private void unsubscribe() {
        if (disposable != null) {
            if (!disposable.isDisposed()) {
                disposable.dispose();
            }
        }
    }

    private void increasePage() {
        currentLastItem += AppConstants.PHOTO_ITEMS_PAGE_LIMIT;
    }

    ///////////////////////////////////////////////////////////////////////////
    // STATES
    ///////////////////////////////////////////////////////////////////////////

    /**
     * Initial paginator state
     */
    private class INITIAL_STATE implements State {

        @Override
        public void refresh() {
            currentState = new REFRESH_STATE();
            viewController.showRefreshProgress(true);
            loadPage(startItem);
        }

        @Override
        public void loadNewPage() {
        }

        @Override
        public void release() {
            currentState = new RELEASED_STATE();
            unsubscribe();
        }

        @Override
        public void newData(List<BaseMewsPhotoItem> list) {
        }

        @Override
        public void error(Throwable throwable) {
        }
    }

    /**
     * State for empty list
     */
    private class EMPTY_DATA_STATE implements State {

        @Override
        public void refresh() {
            currentState = new REFRESH_STATE();
            viewController.showRefreshProgress(true);
            loadPage(startItem);
        }

        @Override
        public void loadNewPage() {
        }

        @Override
        public void release() {
            currentState = new RELEASED_STATE();
            unsubscribe();
        }

        @Override
        public void newData(List<BaseMewsPhotoItem> list) {
        }

        @Override
        public void error(Throwable throwable) {
        }
    }

    /**
     * State for loaded list
     */
    private class DATA_STATE implements State {

        @Override
        public void refresh() {
            currentState = new REFRESH_STATE();
            viewController.showRefreshProgress(true);
            loadPage(startItem);
        }

        @Override
        public void loadNewPage() {
            currentState = new PAGE_PROGRESS_STATE();
            viewController.showPageProgress(true);
            increasePage();
            loadPage(currentLastItem);
        }

        @Override
        public void release() {
            currentState = new RELEASED_STATE();
            unsubscribe();
        }

        @Override
        public void newData(List<BaseMewsPhotoItem> list) {
        }

        @Override
        public void error(Throwable throwable) {
            viewController.showError(throwable);
        }
    }

    /**
     * State for refreshing list
     */
    private class REFRESH_STATE implements State {

        @Override
        public void refresh() {
        }

        @Override
        public void loadNewPage() {
        }

        @Override
        public void release() {
            currentState = new RELEASED_STATE();
            unsubscribe();
        }

        @Override
        public void newData(List<BaseMewsPhotoItem> list) {
            if (!list.isEmpty()) {
                currentState = new DATA_STATE();
                currentData.clear();
                currentData = list;
                currentLastItem = startItem + AppConstants.PHOTO_ITEMS_PAGE_LIMIT;
                viewController.showRefreshProgress(false);
                viewController.showEmptyView(false);
                viewController.showList(true, currentData);
            } else {
                currentState = new EMPTY_DATA_STATE();
                viewController.showEmptyView(true);

                currentData.clear();
                viewController.showList(false, null);
                viewController.showRefreshProgress(false);
            }
        }

        @Override
        public void error(Throwable throwable) {
            currentState = new DATA_STATE();
            viewController.showRefreshProgress(false);
            viewController.showError(throwable);
        }
    }

    /**
     * State for progress page
     */
    private class PAGE_PROGRESS_STATE implements State {

        @Override
        public void refresh() {
            currentState = new REFRESH_STATE();
            viewController.showPageProgress(false);
            viewController.showRefreshProgress(true);
            loadPage(startItem);
        }

        @Override
        public void loadNewPage() {
        }

        @Override
        public void release() {
            currentState = new RELEASED_STATE();
            unsubscribe();
        }

        @Override
        public void newData(List<BaseMewsPhotoItem> list) {
            if (!list.isEmpty()) {
                currentState = new DATA_STATE();
                currentData = list;
                viewController.showPageProgress(false);
                viewController.showList(true, currentData);
            } else {
                currentState = new ALL_DATA_STATE();
                viewController.showPageProgress(false);
            }
        }

        @Override
        public void error(Throwable throwable) {
            currentState = new DATA_STATE();
            viewController.showPageProgress(false);
            viewController.showError(throwable);
        }
    }

    /**
     * State for all available data is loaded
     */
    private class ALL_DATA_STATE implements State {

        @Override
        public void refresh() {
            currentState = new REFRESH_STATE();
            viewController.showRefreshProgress(true);
            loadPage(startItem);
        }

        @Override
        public void loadNewPage() {
        }

        @Override
        public void release() {
            currentState = new RELEASED_STATE();
            unsubscribe();
        }

        @Override
        public void newData(List<BaseMewsPhotoItem> list) {
        }

        @Override
        public void error(Throwable throwable) {
        }
    }

    /**
     * State for paginator released
     */
    private class RELEASED_STATE implements State {

        @Override
        public void refresh() {
        }

        @Override
        public void loadNewPage() {
        }

        @Override
        public void release() {
        }

        @Override
        public void newData(List<BaseMewsPhotoItem> list) {
        }

        @Override
        public void error(Throwable throwable) {
        }
    }
}
