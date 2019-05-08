package dev.gladkowski.mews.di.app.module;

import javax.inject.Singleton;

import dagger.Module;
import dagger.Provides;
import ru.terrakok.cicerone.Cicerone;
import ru.terrakok.cicerone.NavigatorHolder;
import ru.terrakok.cicerone.Router;

@Module
public interface NavigationModule {

    @Provides
    @Singleton
    static Router provideRouter(Cicerone<Router> cicerone) {
        return cicerone.getRouter();
    }

    @Provides
    @Singleton
    static NavigatorHolder provideNavigatorHolder(Cicerone<Router> cicerone) {
        return cicerone.getNavigatorHolder();
    }

    @Provides
    @Singleton
    static Cicerone<Router> provideCicerone() {
        return Cicerone.create();
    }
}
