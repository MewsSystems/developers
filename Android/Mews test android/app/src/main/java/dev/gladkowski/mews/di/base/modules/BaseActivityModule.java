package dev.gladkowski.mews.di.base.modules;

import android.app.Activity;
import android.content.Context;
import android.support.v4.app.FragmentManager;
import android.support.v7.app.AppCompatActivity;

import javax.inject.Named;

import dagger.Binds;
import dagger.Module;
import dagger.Provides;
import dev.gladkowski.mews.di.base.scopes.ActivityScope;
import dev.gladkowski.mews.di.main.qualifier.ActivityContext;


@Module
public interface BaseActivityModule {

    String ACTIVITY_FRAGMENT_MANAGER = "ACTIVITY_FRAGMENT_MANAGER";

    @Provides
    @ActivityScope
    @Named(ACTIVITY_FRAGMENT_MANAGER)
    static FragmentManager provideActivityFragmentManager(AppCompatActivity appCompatActivity) {
        return appCompatActivity.getSupportFragmentManager();
    }

    @Binds
    Activity activity(AppCompatActivity appCompatActivity);

    @Binds
    @ActivityScope
    @ActivityContext
    Context bindContext(Activity activity);
}
