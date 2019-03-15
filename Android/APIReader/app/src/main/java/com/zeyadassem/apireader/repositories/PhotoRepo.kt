package com.zeyadassem.apireader.repositories

import com.zeyadassem.apireader.network.ReaderApi
import com.zeyadassem.apireader.network.models.PhotoModel
import io.reactivex.Observable
import retrofit2.Retrofit
import javax.inject.Inject

class PhotoRepo @Inject constructor(retrofit: Retrofit){

    var readerApi: ReaderApi = retrofit.create(ReaderApi::class.java)

    fun getPhotos(start: String, limit: String): Observable<ArrayList<PhotoModel>>{
        return readerApi.getPhotosList(start, limit)
    }
}