package dev.gladkowski.mews.presentation.mews;

import android.support.annotation.NonNull;

import com.arellomobile.mvp.InjectViewState;

import java.util.List;

import dev.gladkowski.mews.domain.mews.MewsPhotoInteractor;
import dev.gladkowski.mews.entity.mews.presentation.BaseMewsPhotoItem;
import dev.gladkowski.mews.entity.mews.presentation.MewsPhotoItem;
import dev.gladkowski.mews.presentation.common.activity.BaseNetworkPresenter;
import dev.gladkowski.mews.presentation.common.pagination.ViewController;
import dev.gladkowski.mews.presentation.mews.converter.MewsPhotoItemConverter;
import dev.gladkowski.mews.presentation.mews.pagination.MewsPaginator;
import dev.gladkowski.mews.presentation.mews.pagination.MewsPaginatorImpl;
import dev.gladkowski.mews.presentation.mews.provider.MewsResourceProvider;
import dev.gladkowski.mews.utils.rx.ErrorResourceProvider;
import ru.terrakok.cicerone.Router;

/**
 * Presenter for MewsPhotosFragment
 */
@InjectViewState
public class MewsPhotosPresenter extends BaseNetworkPresenter<MewsPhotosView> {

    @NonNull
    private Router router;
    @NonNull
    private ErrorResourceProvider errorResourceProvider;
    @NonNull
    private MewsResourceProvider resourceProvider;
    @NonNull
    private MewsPhotoInteractor mewsPhotoInteractor;
    @NonNull
    private MewsPhotoItemConverter converter;

    private MewsPaginator paginator;
    private int selectedItemId;

    /**
     * Pagination callback interface for view
     */
    private ViewController<BaseMewsPhotoItem> viewController = new ViewController<BaseMewsPhotoItem>() {
        @Override
        public void showEmptyView(boolean show) {
            if (show) {
                //                showEmptyListView; //TODO: display empty list view
            } else {
                getViewState().clearList();
            }
        }

        @Override
        public void showList(boolean show, List<BaseMewsPhotoItem> list) {
            if (show) {
                if (paginator.isFirstPage()) {
                    if(list.size() > 0){ // shows the detailed image of first loaded item
                        MewsPhotoItem firstItem = (MewsPhotoItem) list.get(0);
                        getViewState().showImage(firstItem.getUrl());
                    }
                    getViewState().showList(list);
                } else {
                    getViewState().addMoreItems(list);
                }
            } else {
                getViewState().clearList();
            }
        }

        @Override
        public void showRefreshProgress(boolean show) {
            if (show) {
                getViewState().onShowLoading();
            } else {
                getViewState().onHideLoading();
            }
        }

        @Override
        public void showPageProgress(boolean show) {
            if (show) {
                getViewState().onShowPageLoading();
            } else {
                getViewState().onHidePageLoading();
            }
        }

        @Override
        public void showError(Throwable throwable) {
            processErrors(throwable);
        }
    };

    public MewsPhotosPresenter(@NonNull Router router,
                               @NonNull ErrorResourceProvider errorResourceProvider,
                               @NonNull MewsResourceProvider resourceProvider,
                               @NonNull MewsPhotoInteractor mewsPhotoInteractor,
                               @NonNull MewsPhotoItemConverter converter) {
        this.router = router;
        this.errorResourceProvider = errorResourceProvider;
        this.resourceProvider = resourceProvider;
        this.mewsPhotoInteractor = mewsPhotoInteractor;
        this.converter = converter;
    }

    @NonNull
    @Override
    public Router getRouter() {
        return router;
    }

    @NonNull
    @Override
    public ErrorResourceProvider getErrorResourceProvider() {
        return errorResourceProvider;
    }

    @Override
    protected void onFirstViewAttach() {
        super.onFirstViewAttach();
    }

    @Override
    public void initData() {
        getViewState().onSetTitle(resourceProvider.getTitle());
        paginator = new MewsPaginatorImpl(mewsPhotoInteractor, viewController, converter);
        paginator.refresh();
    }

    /**
     * Refresh list
     */
    void onRefresh() {
        paginator.refresh();
    }

    /**
     * Load next page
     */
    void loadNextPage() {
        paginator.loadNewPage();
    }

    void onItemClicked(int itemId, String imageUrl) {
        if (selectedItemId != itemId) {
            getViewState().showImage(imageUrl);
        }
        selectedItemId = itemId;
    }

    /**
     * Triggered on back button pressed
     */
    @Override
    public void onBackPressed() {
        getRouter().exit();
    }

    @Override
    public void onDestroy() {
        super.onDestroy();
        if (paginator != null) {
            paginator.release();
        }
    }
}
