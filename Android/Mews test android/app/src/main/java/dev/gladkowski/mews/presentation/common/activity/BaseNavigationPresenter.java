package dev.gladkowski.mews.presentation.common.activity;


import ru.terrakok.cicerone.Router;

/**
 * Base navigation presenter
 *
 * @param <View>
 */
public abstract class BaseNavigationPresenter<View extends BaseMvpView> extends BasePresenter<View> {

    private Router localRouter;

    /**
     * Returns current router
     */
    @Override
    public Router getRouter() {
        return localRouter;
    }

    /**
     * Set router router
     */
    public void setLocalRouter(Router localRouter) {
        this.localRouter = localRouter;
    }

    /**
     * Triggered on back button pressed
     */
    @Override
    public void onBackPressed() {
        getRouter().exit();
    }
}
