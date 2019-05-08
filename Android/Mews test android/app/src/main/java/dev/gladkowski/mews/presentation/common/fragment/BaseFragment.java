package dev.gladkowski.mews.presentation.common.fragment;

import android.app.Activity;
import android.content.Context;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.view.MenuItem;
import android.view.View;
import android.view.inputmethod.InputMethodManager;

import com.arellomobile.mvp.MvpAppCompatFragment;

import javax.inject.Inject;
import javax.inject.Provider;

import butterknife.ButterKnife;
import butterknife.Unbinder;
import dagger.android.support.AndroidSupportInjection;
import dev.gladkowski.mews.presentation.common.ActivityCallback;
import dev.gladkowski.mews.presentation.common.activity.BasePresenter;


/**
 * Base mvp fragment
 *
 * @param <Presenter>
 * @param <BaseView>
 */
public abstract class BaseFragment<Presenter extends BasePresenter, BaseView extends BaseFragmentView>
        extends MvpAppCompatFragment implements BaseFragmentView {

    @Inject
    protected Provider<Presenter> presenterProvider;

    private ActivityCallback activityCallback;

    private Unbinder unbinder;

    @Override
    public void onAttach(Context context) {
        super.onAttach(context);

        AndroidSupportInjection.inject(this);
        try {
            activityCallback = (ActivityCallback) context;
        } catch (ClassCastException exception) {
            throw new ClassCastException(context.toString()
                    + " must implement " + activityCallback.getClass());
        }
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setHasOptionsMenu(true);
    }

    @Override
    public void onViewCreated(@NonNull View view, Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);
        if (getView() != null) {
            unbinder = ButterKnife.bind(this, getView());
        }
    }

    /**
     * Unbind ButterKnife views and hide keyboard
     */
    @Override
    public void onDestroyView() {
        unbinder.unbind();
        hideSoftInput();
        super.onDestroyView();
    }

    ///////////////////////////////////////////////////////////////////////////
    // GETTERS
    ///////////////////////////////////////////////////////////////////////////

    /**
     * Get presenter
     */
    protected abstract Presenter getPresenter();


    ///////////////////////////////////////////////////////////////////////////
    // UI METHODS
    ///////////////////////////////////////////////////////////////////////////

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        switch (item.getItemId()) {
            case android.R.id.home:
                onBackPressed();
                return true;
        }
        return super.onOptionsItemSelected(item);
    }

    /**
     * Hide soft input
     */
    @Override
    public void hideSoftInput() {
        if (getActivity() != null) {
            View focus = getActivity().getCurrentFocus();
            if (focus != null) {
                InputMethodManager inputMethodManager = (InputMethodManager) getActivity().getSystemService(Activity.INPUT_METHOD_SERVICE);
                if (inputMethodManager != null) {
                    inputMethodManager.hideSoftInputFromWindow(focus.getWindowToken(), 0);
                }
            }
        }
    }

    ///////////////////////////////////////////////////////////////////////////
    // MVP
    ///////////////////////////////////////////////////////////////////////////

    /**
     * Show loading view
     */
    @Override
    public void onShowLoading() {
        if (activityCallback != null) {
            activityCallback.onShowLoading();
        }
    }

    /**
     * Hide loading view
     */
    @Override
    public void onHideLoading() {
        if (activityCallback != null) {
            activityCallback.onHideLoading();
        }
    }

    /**
     * Show error
     */
    @Override
    public void onShowError(String message) {
        if (activityCallback != null) {
            activityCallback.onShowError(message);
        }
    }

    /**
     * Set title
     */
    @Override
    public void onSetTitle(String title) {
        if (activityCallback != null) {
            activityCallback.onSetTitle(title);
        }
    }

    /**
     * Triggered on back button pressed
     */
    @Override
    public void onBackPressed() {
        getPresenter().onBackPressed();
    }
}

