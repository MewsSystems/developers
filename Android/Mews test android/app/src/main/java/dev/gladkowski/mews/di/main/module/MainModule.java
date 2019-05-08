package dev.gladkowski.mews.di.main.module;

import android.support.v7.app.AppCompatActivity;

import dagger.Binds;
import dagger.Module;
import dagger.android.ContributesAndroidInjector;
import dev.gladkowski.mews.di.base.modules.BaseActivityModule;
import dev.gladkowski.mews.di.base.modules.BaseFragmentModule;
import dev.gladkowski.mews.di.base.scopes.ActivityScope;
import dev.gladkowski.mews.di.mews.MewsModule;
import dev.gladkowski.mews.di.mews.MewsScope;
import dev.gladkowski.mews.presentation.main.MainActivity;
import dev.gladkowski.mews.presentation.mews.MewsPhotosFragment;

@Module(includes = {BaseActivityModule.class,
        MainPresenterModule.class,
        BaseFragmentModule.class,
        MewsModule.class,
})
public interface MainModule {

    @Binds
    @ActivityScope
    AppCompatActivity bindAppCompatActivity(MainActivity mainActivity);

    @MewsScope
    @ContributesAndroidInjector(modules = MewsModule.class)
    MewsPhotosFragment mewsFragmentInjector();
}
