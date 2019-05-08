package dev.gladkowski.mews.di;


import dagger.Module;
import dagger.android.ContributesAndroidInjector;
import dev.gladkowski.mews.di.base.scopes.ActivityScope;
import dev.gladkowski.mews.di.main.module.MainModule;
import dev.gladkowski.mews.presentation.main.MainActivity;


@Module
public interface ActivityInjectionModule {

    @ActivityScope
    @ContributesAndroidInjector(modules = {MainModule.class})
    MainActivity mainActivityInjector();

}
