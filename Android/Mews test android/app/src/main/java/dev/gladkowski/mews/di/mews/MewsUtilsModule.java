package dev.gladkowski.mews.di.mews;

import android.content.Context;

import dagger.Module;
import dagger.Provides;
import dev.gladkowski.mews.data.repository.mews.converter.MewsPhotoResponseConverter;
import dev.gladkowski.mews.data.repository.mews.converter.MewsPhotoResponseConverterImpl;
import dev.gladkowski.mews.presentation.mews.converter.MewsPhotoItemConverter;
import dev.gladkowski.mews.presentation.mews.converter.MewsPhotoItemConverterImpl;
import dev.gladkowski.mews.presentation.mews.provider.MewsResourceProvider;
import dev.gladkowski.mews.presentation.mews.provider.MewsResourceProviderImpl;


@Module
public interface MewsUtilsModule {

    @Provides
    static MewsResourceProvider provideMewsResourceProvider(Context context) {
        return new MewsResourceProviderImpl(context);
    }

    @Provides
    static MewsPhotoResponseConverter provideMewsPhotoResponseConverter() {
        return new MewsPhotoResponseConverterImpl();
    }

    @Provides
    static MewsPhotoItemConverter provideMewsPhotoItemConverter() {
        return new MewsPhotoItemConverterImpl();
    }
}
