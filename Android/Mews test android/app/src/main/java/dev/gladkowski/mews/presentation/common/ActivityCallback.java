package dev.gladkowski.mews.presentation.common;

public interface ActivityCallback {

    /**
     * Set toolbar title
     */
    void onSetTitle(String title);

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

}
