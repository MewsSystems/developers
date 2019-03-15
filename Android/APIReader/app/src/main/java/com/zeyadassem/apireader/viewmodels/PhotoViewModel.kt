package com.zeyadassem.apireader.viewmodels

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.Transformations
import androidx.lifecycle.ViewModel
import androidx.paging.LivePagedListBuilder
import androidx.paging.PagedList
import com.zeyadassem.apireader.di.DaggerPhotoRepoComponent
import com.zeyadassem.apireader.network.models.PhotoModel
import com.zeyadassem.apireader.paging.PhotosDataSource
import com.zeyadassem.apireader.paging.PhotosDataSourceFactory
import com.zeyadassem.apireader.repositories.PhotoRepo
import io.reactivex.disposables.CompositeDisposable
import javax.inject.Inject

class PhotoViewModel: ViewModel() {

    var progressLiveData: LiveData<String> = MutableLiveData<String>()
    var compositeDisposable = CompositeDisposable()
    lateinit var photoListLiveData : LiveData<PagedList<PhotoModel>>
    lateinit var photosDataSourceFactory: PhotosDataSourceFactory

    @Inject
    lateinit var photoRepo: PhotoRepo

    init {
        DaggerPhotoRepoComponent.create().inject(this)
        photosDataSourceFactory = PhotosDataSourceFactory(photoRepo, compositeDisposable)
        initializePaging(photosDataSourceFactory)
    }

    private fun initializePaging(photosDataSourceFactory: PhotosDataSourceFactory) {
        val pagedListConfig = PagedList.Config.Builder()
            .setEnablePlaceholders(false)
            .setPageSize(30)
            .build()
        photoListLiveData = LivePagedListBuilder(photosDataSourceFactory, pagedListConfig)
            .build()

        progressLiveData = Transformations.switchMap(photosDataSourceFactory.getMutableLiveData(),PhotosDataSource::progressLiveStatus)

    }



    fun listIsEmpty(): Boolean {
        return photoListLiveData.value?.isEmpty() ?: true
    }

    override fun onCleared() {
        super.onCleared()
        compositeDisposable.clear()
    }

    fun retry() {
        photosDataSourceFactory.liveData.value?.retry()
    }
}