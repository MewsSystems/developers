package dev.gladkowski.mews.di.mews;

import dagger.Module;
import dagger.Provides;
import dev.gladkowski.mews.data.network.MewsPhotoApi;
import dev.gladkowski.mews.data.repository.mews.MewsPhotoRepository;
import dev.gladkowski.mews.data.repository.mews.MewsPhotoRepositoryImpl;
import dev.gladkowski.mews.data.repository.mews.converter.MewsPhotoResponseConverter;

@Module
public interface MewsRepositoryModule {

    @Provides
    static MewsPhotoRepository provideMewsRepository(MewsPhotoApi mewsPhotoApi,
                                                     MewsPhotoResponseConverter converter) {
        return new MewsPhotoRepositoryImpl(mewsPhotoApi, converter);
    }
}
