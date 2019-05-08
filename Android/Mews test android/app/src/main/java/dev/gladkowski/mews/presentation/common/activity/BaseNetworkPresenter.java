package dev.gladkowski.mews.presentation.common.activity;


import dev.gladkowski.mews.utils.rx.ErrorResourceProvider;
import dev.gladkowski.mews.utils.rx.ErrorUtils;
import io.reactivex.annotations.NonNull;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.disposables.Disposable;

/**
 * Base presenter for working with network
 *
 * @param <View>
 */
public abstract class BaseNetworkPresenter<View extends BaseMvpView> extends BaseNavigationPresenter<View> {

    private CompositeDisposable compositeDisposable = new CompositeDisposable();

    public abstract ErrorResourceProvider getErrorResourceProvider();

    protected void unsubscribeOnDestroy(@NonNull Disposable disposable) {
        compositeDisposable.add(disposable);
    }

    /**
     * Init primary data
     */
    @Override
    public void initData() {
    }

    /**
     * Clear all subscriptions
     */
    @Override
    public void onDestroy() {
        super.onDestroy();
        compositeDisposable.clear();
    }

    protected void processErrors(Throwable throwable) {
        ErrorUtils errorUtils = new ErrorUtils();
        errorUtils.processErrors(throwable, getRouter(), getErrorResourceProvider(), getViewState());
    }
}
