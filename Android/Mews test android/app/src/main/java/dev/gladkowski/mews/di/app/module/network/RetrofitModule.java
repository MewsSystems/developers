package dev.gladkowski.mews.di.app.module.network;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

import org.joda.time.DateTime;

import javax.inject.Singleton;

import dagger.Module;
import dagger.Provides;
import dev.gladkowski.mews.utils.date.DateTimeResponseConverter;
import dev.gladkowski.mews.utils.date.DateTimeResponseConverterImpl;
import okhttp3.logging.HttpLoggingInterceptor;
import retrofit2.Converter;
import retrofit2.converter.gson.GsonConverterFactory;


/**
 * Модуль предоставляющий Retrofit
 */
@Module
public interface RetrofitModule {

    @Provides
    @Singleton
    static HttpLoggingInterceptor provideLogger() {
        return new HttpLoggingInterceptor()
                .setLevel(HttpLoggingInterceptor.Level.BODY);
    }

    @Provides
    @Singleton
    static Converter.Factory provideConverterFactory(Gson gson) {
        return GsonConverterFactory.create(gson);
    }

    @Provides
    @Singleton
    static Gson provideGson(DateTimeResponseConverter dateTimeConverter) {
        return new GsonBuilder()
                .registerTypeAdapter(DateTime.class, dateTimeConverter)
                .create();
    }

    @Provides
    static DateTimeResponseConverter provideDateTimeConverter() {
        return new DateTimeResponseConverterImpl();
    }
}
