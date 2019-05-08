package dev.gladkowski.mews.di.mews;

import android.support.v4.app.Fragment;

import dagger.Binds;
import dagger.Module;
import dev.gladkowski.mews.di.base.modules.BaseFragmentModule;
import dev.gladkowski.mews.presentation.mews.MewsPhotosFragment;

@Module(includes = {BaseFragmentModule.class,
        MewsPresenterModule.class,
        MewsInteractorModule.class,
        MewsRepositoryModule.class,
        MewsUtilsModule.class})
public interface MewsModule {

    @Binds
    Fragment bindFragment(MewsPhotosFragment mewsFragment);
}
