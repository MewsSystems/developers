package dev.gladkowski.mews.presentation.common.fragment;


import dev.gladkowski.mews.presentation.common.activity.BaseMvpView;

/**
 * Base interface for fragments
 */
public interface BaseFragmentView extends BaseMvpView {

    /**
     * Triggered on back button pressed
     */
    void onBackPressed();
}
