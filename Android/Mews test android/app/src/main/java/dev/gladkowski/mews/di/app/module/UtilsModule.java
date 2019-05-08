package dev.gladkowski.mews.di.app.module;


import javax.inject.Singleton;

import dagger.Binds;
import dagger.Module;
import dagger.Provides;
import dev.gladkowski.mews.utils.imageloader.ImageLoader;
import dev.gladkowski.mews.utils.imageloader.glide.GlideImageLoaderImpl;
import dev.gladkowski.mews.utils.rx.CompletableErrorHandler;
import dev.gladkowski.mews.utils.rx.ErrorProcessing;
import dev.gladkowski.mews.utils.rx.ErrorResourceProvider;
import dev.gladkowski.mews.utils.rx.ErrorResourceProviderImpl;
import dev.gladkowski.mews.utils.rx.RxUtils;
import dev.gladkowski.mews.utils.rx.SingleErrorHandler;


@Module
public interface UtilsModule {

    @Provides
    @Singleton
    static ErrorProcessing provideErrorProcessing(ErrorResourceProvider errorResourceProvider) {
        return new ErrorProcessing(errorResourceProvider);
    }

    @Provides
    @Singleton
    static SingleErrorHandler provideSingleErrorHandler(ErrorProcessing errorProcessing) {
        return new SingleErrorHandler(errorProcessing);
    }

    @Provides
    @Singleton
    static CompletableErrorHandler provideCompletableErrorHandler(ErrorProcessing errorProcessing) {
        return new CompletableErrorHandler(errorProcessing);
    }

    @Provides
    @Singleton
    static RxUtils provideRxUtils() {
        return new RxUtils();
    }

    @Binds
    @Singleton
    ErrorResourceProvider provideErrorResourceProvider(ErrorResourceProviderImpl resourceProvider);

    @Binds
    @Singleton
    ImageLoader bindImageLoader(GlideImageLoaderImpl loader);

}
