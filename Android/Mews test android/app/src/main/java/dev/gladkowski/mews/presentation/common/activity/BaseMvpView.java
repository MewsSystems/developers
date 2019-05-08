package dev.gladkowski.mews.presentation.common.activity;

import com.arellomobile.mvp.MvpView;

/**
 * Base interface for MVP
 */
public interface BaseMvpView extends MvpView {

    /**
     * Show loading view
     */
    void onShowLoading();

    /**
     * Hide loading view
     */
    void onHideLoading();

    /**
     * Show error
     */
    void onShowError(String message);

    /**
     * Hide soft input
     */
    void hideSoftInput();

    /**
     * Set title
     */
    void onSetTitle(String title);

}