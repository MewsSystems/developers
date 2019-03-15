package com.zeyadassem.apireader.paging

import androidx.lifecycle.MutableLiveData
import androidx.paging.DataSource
import com.zeyadassem.apireader.network.models.PhotoModel
import com.zeyadassem.apireader.repositories.PhotoRepo
import io.reactivex.disposables.CompositeDisposable

class PhotosDataSourceFactory(val repo: PhotoRepo,
                              val compositeDisposable: CompositeDisposable
                                ): DataSource.Factory<Int, PhotoModel>() {

    var liveData = MutableLiveData<PhotosDataSource>()

    override fun create(): DataSource<Int, PhotoModel> {
        val photoDataSource = PhotosDataSource(repo, compositeDisposable)
        liveData.postValue(photoDataSource)
        return photoDataSource
    }

    fun getMutableLiveData(): MutableLiveData<PhotosDataSource>{
        return liveData
    }
}