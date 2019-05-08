package dev.gladkowski.mews.di.base.modules;

import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;

import javax.inject.Named;

import dagger.Module;
import dagger.Provides;

@Module
public interface BaseFragmentModule {

    String FRAGMENT = "BaseFragmentModule.fragment";

    String CHILD_FRAGMENT_MANAGER = "BaseFragmentModule.childFragmentManager";

    @Provides
    @Named(CHILD_FRAGMENT_MANAGER)
    static FragmentManager provideChildFragmentManager(@Named(FRAGMENT) Fragment fragment) {
        return fragment.getChildFragmentManager();
    }
}
