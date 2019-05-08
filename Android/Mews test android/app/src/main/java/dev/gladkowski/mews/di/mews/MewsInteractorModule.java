package dev.gladkowski.mews.di.mews;

import dagger.Module;
import dagger.Provides;
import dev.gladkowski.mews.data.repository.mews.MewsPhotoRepository;
import dev.gladkowski.mews.domain.mews.MewsPhotoInteractor;
import dev.gladkowski.mews.domain.mews.MewsPhotoInteractorImpl;
import dev.gladkowski.mews.utils.rx.RxUtils;
import dev.gladkowski.mews.utils.rx.SingleErrorHandler;


@Module
public interface MewsInteractorModule {

    @Provides
    static MewsPhotoInteractor provideMewsInteractor(MewsPhotoRepository mewsPhotoRepository,
                                                     SingleErrorHandler singleErrorHandler,
                                                     RxUtils rxUtils) {
        return new MewsPhotoInteractorImpl(mewsPhotoRepository, singleErrorHandler, rxUtils);
    }
}
