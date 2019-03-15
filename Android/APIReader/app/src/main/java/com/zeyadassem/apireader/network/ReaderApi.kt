package com.zeyadassem.apireader.network

import com.zeyadassem.apireader.network.models.PhotoModel
import com.zeyadassem.apireader.ui.utils.AppConstants
import io.reactivex.Observable
import retrofit2.http.GET
import retrofit2.http.Query

interface ReaderApi {
    @GET(AppConstants.PHOTO_LIST_PATH)
    fun getPhotosList(@Query("_start") start: String, @Query("_limit") limit: String): Observable<ArrayList<PhotoModel>>
}