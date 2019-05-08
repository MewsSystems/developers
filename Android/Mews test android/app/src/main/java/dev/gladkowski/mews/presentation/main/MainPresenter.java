package dev.gladkowski.mews.presentation.main;

import com.arellomobile.mvp.InjectViewState;

import dev.gladkowski.mews.presentation.common.activity.BasePresenter;
import dev.gladkowski.mews.presentation.common.constants.MainScreens;
import io.reactivex.annotations.NonNull;
import ru.terrakok.cicerone.Router;

/**
 * Presenter for MainActivity
 */
@InjectViewState
public class MainPresenter extends BasePresenter<MainView> {

    /**
     * Router for navigation
     */
    @NonNull
    private Router router;

    public MainPresenter(@NonNull Router router) {
        this.router = router;
    }

    /**
     * Init screen at first launch
     */
    @Override
    public void initData() {
        getRouter().replaceScreen(MainScreens.MEWS_PHOTOS_PAGE);
    }

    @Override
    public void onBackPressed() {
        getRouter().exit();
    }

    @Override
    @NonNull
    public Router getRouter() {
        return router;
    }
}

