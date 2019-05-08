package dev.gladkowski.mews.di.app.module.network;

import java.util.concurrent.TimeUnit;

import javax.inject.Singleton;

import dagger.Module;
import dagger.Provides;
import dev.gladkowski.mews.BuildConfig;
import dev.gladkowski.mews.entity.app.data.AppConfig;
import okhttp3.OkHttpClient;
import okhttp3.logging.HttpLoggingInterceptor;
import retrofit2.Converter;
import retrofit2.Retrofit;
import retrofit2.adapter.rxjava2.RxJava2CallAdapterFactory;

@Module
public interface CommonNetworkModule {

    @Provides
    @Singleton
    static OkHttpClient provideOkHttpClient(HttpLoggingInterceptor loggingInterceptor) {
        return new OkHttpClient.Builder()
                .addInterceptor(loggingInterceptor)
                .connectTimeout(AppConfig.DEFAULT_TIMEOUT, TimeUnit.SECONDS)
                .readTimeout(AppConfig.DEFAULT_TIMEOUT, TimeUnit.SECONDS)
                .build();
    }

    @Provides
    @Singleton
    static Retrofit.Builder provideRetrofitBuilder(Converter.Factory converterFactory, OkHttpClient client) {
        return new Retrofit.Builder()
                .client(client)
                .addCallAdapterFactory(RxJava2CallAdapterFactory.create())
                .addConverterFactory(converterFactory);
    }

    @Provides
    static Retrofit provideRetrofit(Retrofit.Builder builder) {
        return builder.baseUrl(BuildConfig.baseUrl).build();
    }
}
