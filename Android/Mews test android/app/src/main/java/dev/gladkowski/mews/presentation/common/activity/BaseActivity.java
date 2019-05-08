package dev.gladkowski.mews.presentation.common.activity;

import android.app.Activity;
import android.os.Bundle;
import android.support.annotation.LayoutRes;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.view.View;
import android.view.inputmethod.InputMethodManager;

import com.arellomobile.mvp.MvpAppCompatActivity;

import javax.inject.Inject;
import javax.inject.Named;
import javax.inject.Provider;

import butterknife.BindView;
import dagger.android.AndroidInjection;
import dagger.android.AndroidInjector;
import dagger.android.DispatchingAndroidInjector;
import dagger.android.support.HasSupportFragmentInjector;
import dev.gladkowski.mews.R;
import dev.gladkowski.mews.di.base.modules.BaseActivityModule;
import dev.gladkowski.mews.presentation.common.ActivityCallback;
import ru.terrakok.cicerone.Navigator;
import ru.terrakok.cicerone.NavigatorHolder;

/**
 * Base mvp activity with auto dagger injection
 *
 * @param <Presenter>
 * @param <BaseView>
 */
public abstract class BaseActivity<Presenter extends BasePresenter, BaseView extends BaseMvpView> extends MvpAppCompatActivity
        implements BaseMvpView, ActivityCallback, HasSupportFragmentInjector {

    @Inject
    protected Provider<Presenter> presenterProvider;

    protected Navigator localNavigator;

    @Inject
    @Named(BaseActivityModule.ACTIVITY_FRAGMENT_MANAGER)
    protected FragmentManager fragmentManager;

    @Inject
    DispatchingAndroidInjector<Fragment> fragmentDispatchingAndroidInjector;

    @Override
    public AndroidInjector<Fragment> supportFragmentInjector() {
        return fragmentDispatchingAndroidInjector;
    }

    @BindView(R.id.view_progress_layout)
    protected View progressView;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        AndroidInjection.inject(this);
        super.onCreate(savedInstanceState);
        setContentView(getLayoutActivity());
    }

    @Override
    protected void onResumeFragments() {
        super.onResumeFragments();
        getNavigationHolder().setNavigator(getLocalNavigator());
    }


    @Override
    protected void onPause() {
        getNavigationHolder().removeNavigator();
        super.onPause();
    }

    ///////////////////////////////////////////////////////////////////////////
    // GETTERS
    ///////////////////////////////////////////////////////////////////////////

    /**
     * Get resource for showing activity
     */
    @LayoutRes
    protected abstract int getLayoutActivity();

    /**
     * Get current navigator
     */
    protected abstract Navigator getLocalNavigator();

    /**
     * Get presenter
     */
    protected abstract Presenter getPresenter();

    /**
     * Get navigator holder
     */
    protected abstract NavigatorHolder getNavigationHolder();

    ///////////////////////////////////////////////////////////////////////////
    // MVP
    ///////////////////////////////////////////////////////////////////////////

    /**
     * Show loading
     */
    @Override
    public void onShowLoading() {
        if (progressView != null) {
            progressView.setVisibility(View.VISIBLE);
        }
    }

    /**
     * Hide loading
     */
    @Override
    public void onHideLoading() {
        if (progressView != null) {
            progressView.setVisibility(View.GONE);
        }
    }

    /**
     * Hide soft input
     */
    @Override
    public void hideSoftInput() {
        InputMethodManager inputMethodManager = (InputMethodManager) getSystemService(Activity.INPUT_METHOD_SERVICE);
        if (inputMethodManager != null && getCurrentFocus() != null) {
            inputMethodManager.hideSoftInputFromWindow(getCurrentFocus().getWindowToken(), 0);
        }
    }

    /**
     * Show error
     */
    @Override
    public void onShowError(String message) {

    }
}