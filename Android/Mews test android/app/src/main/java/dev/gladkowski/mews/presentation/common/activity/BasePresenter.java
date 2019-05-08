package dev.gladkowski.mews.presentation.common.activity;

import com.arellomobile.mvp.MvpPresenter;

import io.reactivex.annotations.NonNull;
import ru.terrakok.cicerone.Router;

/**
 * Base mvp presenter
 *
 * @param <View>
 */
public abstract class BasePresenter<View extends BaseMvpView> extends MvpPresenter<View> {

    /**
     * Triggered on back button pressed
     */
    public abstract void onBackPressed();

    /**
     * Returns current router
     */
    @NonNull
    public abstract Router getRouter();

    /**
     * Init primary data
     */
    public abstract void initData();

    @Override
    protected void onFirstViewAttach() {
        super.onFirstViewAttach();
        initData();
    }
}
