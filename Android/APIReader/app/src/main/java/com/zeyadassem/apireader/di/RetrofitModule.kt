package com.zeyadassem.apireader.di

import com.google.gson.Gson
import com.google.gson.GsonBuilder
import com.zeyadassem.apireader.ui.utils.AppConstants
import dagger.Module
import dagger.Provides
import retrofit2.Retrofit
import retrofit2.adapter.rxjava2.RxJava2CallAdapterFactory
import retrofit2.converter.gson.GsonConverterFactory
import javax.inject.Singleton

@Module
class RetrofitModule {

    @Singleton
    @Provides
    fun provideGson(): Gson {
        return GsonBuilder().create()
    }

    @Singleton
    @Provides
    fun provideRetrofit(gson: Gson): Retrofit {
        return Retrofit.Builder()
            .baseUrl(AppConstants.BASE_URL)
            .addConverterFactory(GsonConverterFactory.create(gson))
            . addCallAdapterFactory(RxJava2CallAdapterFactory.create())
            .build()
    }
}