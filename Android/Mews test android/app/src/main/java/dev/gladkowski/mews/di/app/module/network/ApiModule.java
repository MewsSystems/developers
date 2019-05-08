package dev.gladkowski.mews.di.app.module.network;

import javax.inject.Singleton;

import dagger.Module;
import dagger.Provides;
import dev.gladkowski.mews.data.network.MewsPhotoApi;
import retrofit2.Retrofit;

@Module(includes = {RetrofitModule.class, CommonNetworkModule.class})
public interface ApiModule {

    @Singleton
    @Provides
    static MewsPhotoApi provideMewsApi(Retrofit retrofit) {
        return retrofit.create(MewsPhotoApi.class);
    }
}
