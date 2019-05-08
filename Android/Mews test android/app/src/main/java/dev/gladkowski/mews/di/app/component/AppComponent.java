package dev.gladkowski.mews.di.app.component;

import javax.inject.Singleton;

import dagger.Component;
import dagger.android.AndroidInjector;
import dev.gladkowski.mews.App;
import dev.gladkowski.mews.di.ActivityInjectionModule;
import dev.gladkowski.mews.di.app.module.AppModule;
import dev.gladkowski.mews.di.app.module.NavigationModule;
import dev.gladkowski.mews.di.app.module.UtilsModule;
import dev.gladkowski.mews.di.app.module.network.ApiModule;


@Singleton
@Component(modules = {AppModule.class, ActivityInjectionModule.class, NavigationModule.class,
        UtilsModule.class, ApiModule.class})
public interface AppComponent extends AndroidInjector<App> {

    @Component.Builder
    abstract class Builder extends AndroidInjector.Builder<App> {
    }
}
