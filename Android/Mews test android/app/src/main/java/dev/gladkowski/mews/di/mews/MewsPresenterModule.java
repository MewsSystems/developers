package dev.gladkowski.mews.di.mews;

import dagger.Module;
import dagger.Provides;
import dev.gladkowski.mews.domain.mews.MewsPhotoInteractor;
import dev.gladkowski.mews.presentation.mews.MewsPhotosPresenter;
import dev.gladkowski.mews.presentation.mews.converter.MewsPhotoItemConverter;
import dev.gladkowski.mews.presentation.mews.provider.MewsResourceProvider;
import dev.gladkowski.mews.utils.rx.ErrorResourceProvider;
import ru.terrakok.cicerone.Router;

@Module
public interface MewsPresenterModule {

    @Provides
    static MewsPhotosPresenter provideMewsPresenter(Router router,
                                                    ErrorResourceProvider errorResourceProvider,
                                                    MewsResourceProvider resourceProvider,
                                                    MewsPhotoInteractor mewsPhotoInteractor,
                                                    MewsPhotoItemConverter converter) {
        return new MewsPhotosPresenter(router, errorResourceProvider,
                resourceProvider, mewsPhotoInteractor, converter);
    }
}
