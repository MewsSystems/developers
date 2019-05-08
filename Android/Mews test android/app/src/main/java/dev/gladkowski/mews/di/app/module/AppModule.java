package dev.gladkowski.mews.di.app.module;

import android.content.Context;

import javax.inject.Singleton;

import dagger.Binds;
import dagger.Module;
import dagger.android.support.AndroidSupportInjectionModule;
import dev.gladkowski.mews.App;

@Module(includes = AndroidSupportInjectionModule.class)
public interface AppModule {

    @Binds
    @Singleton
    Context bindContext(App app);
}
