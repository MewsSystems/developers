package dev.gladkowski.mews.di.main.module;

import dagger.Module;
import dagger.Provides;
import dev.gladkowski.mews.di.base.scopes.ActivityScope;
import dev.gladkowski.mews.presentation.main.MainPresenter;
import ru.terrakok.cicerone.Router;

@Module
public interface MainPresenterModule {

    @Provides
    @ActivityScope
    static MainPresenter provideMainPresenter(Router router) {
        return new MainPresenter(router);
    }
}
