package com.marekfeifrlik.android.mewsphotos.api

import com.marekfeifrlik.android.mewsphotos.data.PhotoEntity
import io.reactivex.Observable
import retrofit2.Retrofit
import retrofit2.adapter.rxjava2.RxJava2CallAdapterFactory
import retrofit2.converter.gson.GsonConverterFactory
import retrofit2.http.GET


interface PhotosApi {

//    @GET("photos?_start=0&_limit=30")
//    fun getPhotos(): Observable<List<PhotoEntity>>

    @GET("photos")
    fun getPhotos(): Observable<List<PhotoEntity>>


    companion object {
        fun create(): PhotosApi {

            val retrofit = Retrofit.Builder()
                .addCallAdapterFactory(RxJava2CallAdapterFactory.create())
                .addConverterFactory(GsonConverterFactory.create())
                .baseUrl("https://jsonplaceholder.typicode.com/")
                .build()

            return retrofit.create(PhotosApi::class.java)
        }
    }

}