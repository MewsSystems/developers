package dev.gladkowski.mews.presentation.mews;

import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.v4.widget.SwipeRefreshLayout;
import android.support.v7.widget.DefaultItemAnimator;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.support.v7.widget.Toolbar;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;

import com.arellomobile.mvp.presenter.InjectPresenter;
import com.arellomobile.mvp.presenter.ProvidePresenter;

import java.util.List;

import javax.inject.Inject;

import butterknife.BindView;
import dev.gladkowski.mews.R;
import dev.gladkowski.mews.entity.mews.presentation.BaseMewsPhotoItem;
import dev.gladkowski.mews.presentation.common.fragment.BaseFragment;
import dev.gladkowski.mews.presentation.mews.adapter.MewsPhotosAdapter;
import dev.gladkowski.mews.presentation.mews.adapter.callback.MewsPhotoItemCallback;
import dev.gladkowski.mews.presentation.mews.constants.MewsPhotosConstants;
import dev.gladkowski.mews.utils.imageloader.ImageLoader;

/**
 * Fragment with list of mews photos
 */
public class MewsPhotosFragment extends BaseFragment<MewsPhotosPresenter, MewsPhotosView> implements MewsPhotosView,
        MewsPhotoItemCallback {

    @InjectPresenter
    MewsPhotosPresenter mewsPhotosPresenter;

    @Inject
    ImageLoader imageLoader;

    @BindView(R.id.toolbar)
    Toolbar toolbar;
    @BindView(R.id.recycler_view)
    RecyclerView recyclerView;
    @BindView(R.id.refresh_layout)
    SwipeRefreshLayout swipeRefreshLayout;
    @BindView(R.id.image_view)
    ImageView imageView;

    private MewsPhotosAdapter mewsPhotosAdapter;

    public MewsPhotosFragment() {
    }

    public static MewsPhotosFragment newInstance() {
        MewsPhotosFragment fragment = new MewsPhotosFragment();
        Bundle args = new Bundle();
        fragment.setArguments(args);
        return fragment;
    }

    @ProvidePresenter
    MewsPhotosPresenter provideMewsPhotoPresenter() {
        return presenterProvider.get();
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container,
                             @Nullable Bundle savedInstanceState) {
        return inflater.inflate(R.layout.fragment_mews_photos, container, false);
    }

    @Override
    public void onViewCreated(@NonNull View view, @Nullable Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);

        initList();
    }

    /**
     * GridLayoutManager initialization
     */
    private void initList() {
        mewsPhotosAdapter = new MewsPhotosAdapter(this, imageLoader);
        recyclerView.setAdapter(mewsPhotosAdapter);
        LinearLayoutManager layoutManager = new LinearLayoutManager(getContext());

        recyclerView.setLayoutManager(layoutManager);
        recyclerView.setItemAnimator(new DefaultItemAnimator());
        recyclerView.addOnScrollListener(new RecyclerView.OnScrollListener() {
            @Override
            public void onScrolled(RecyclerView recyclerView, int dx, int dy) {
                super.onScrolled(recyclerView, dx, dy);
                int visibleItemCount = layoutManager.getChildCount();
                int totalItemCount = layoutManager.getItemCount();
                int firstVisibleItemPosition = layoutManager.findFirstVisibleItemPosition();

                if ((visibleItemCount + firstVisibleItemPosition) >= totalItemCount
                        && firstVisibleItemPosition >= 0
                        && totalItemCount >= MewsPhotosConstants.ITEMS_PER_PAGE) {
                    getPresenter().loadNextPage();
                }
            }
        });

        swipeRefreshLayout.setOnRefreshListener(() -> getPresenter().onRefresh());
    }

    ///////////////////////////////////////////////////////////////////////////
    // GETTERS
    ///////////////////////////////////////////////////////////////////////////

    @Override
    protected MewsPhotosPresenter getPresenter() {
        return mewsPhotosPresenter;
    }

    ///////////////////////////////////////////////////////////////////////////
    // MVP
    ///////////////////////////////////////////////////////////////////////////

    @Override
    public void onSetTitle(String title) {
        toolbar.setTitle(title);
    }

    @Override
    public void showImage(String imageUrl) {
        imageLoader.setImageCenterCrop(imageView, imageUrl);
    }

    @Override
    public void showList(List<BaseMewsPhotoItem> items) {
        mewsPhotosAdapter.addItems(items);
    }

    @Override
    public void addMoreItems(List<BaseMewsPhotoItem> items) {
        mewsPhotosAdapter.addMoreItems(items);
    }

    @Override
    public void clearList() {
        mewsPhotosAdapter.clearList();
    }

    @Override
    public void onShowLoading() {
        swipeRefreshLayout.setRefreshing(true);
    }

    @Override
    public void onHideLoading() {
        swipeRefreshLayout.setRefreshing(false);
    }

    @Override
    public void onShowPageLoading() {
        mewsPhotosAdapter.showPageLoading();
    }

    @Override
    public void onHidePageLoading() {
        mewsPhotosAdapter.hidePageLoading();
    }

    ///////////////////////////////////////////////////////////////////////////
    // UI METHODS
    ///////////////////////////////////////////////////////////////////////////

    @Override
    public void onItemClick(int itemId, String imageUrl) {
        getPresenter().onItemClicked(itemId, imageUrl);
    }

    @Override
    public void onDestroyView() {
        super.onDestroyView();
    }
}
