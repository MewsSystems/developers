package com.zeyadassem.apireader.paging

import androidx.lifecycle.MutableLiveData
import androidx.paging.PageKeyedDataSource
import com.zeyadassem.apireader.network.models.PhotoModel
import com.zeyadassem.apireader.repositories.PhotoRepo
import com.zeyadassem.apireader.ui.utils.AppConstants
import io.reactivex.Completable
import io.reactivex.android.schedulers.AndroidSchedulers
import io.reactivex.disposables.CompositeDisposable
import io.reactivex.functions.Action
import io.reactivex.schedulers.Schedulers

class PhotosDataSource (val repo: PhotoRepo,
                        val compositeDisposable: CompositeDisposable
): PageKeyedDataSource<Int, PhotoModel>() {

    var progressLiveStatus = MutableLiveData<String>()
    var sourceIndex = 0
    val pageSize = 30

    private var retryCompletable: Completable? = null

    override fun loadInitial(params: LoadInitialParams<Int>, callback: LoadInitialCallback<Int, PhotoModel>) {
        repo.getPhotos(sourceIndex.toString(), pageSize.toString()).
                doOnSubscribe {
                    compositeDisposable.add(it)
                    progressLiveStatus.postValue(AppConstants.INITIAL_LOADING_STATUS)
                }.subscribe({
                    progressLiveStatus.postValue(AppConstants.LOADED_STATUS)
                    sourceIndex+=pageSize
                    callback.onResult(it, null, sourceIndex)
                },{
                    progressLiveStatus.postValue(AppConstants.ERROR_STATUS)
                    setRetry(Action { loadInitial(params, callback) })
                })
    }

    override fun loadAfter(params: LoadParams<Int>, callback: LoadCallback<Int, PhotoModel>) {
        repo.getPhotos(params.key.toString(), pageSize.toString())
            .doOnSubscribe{
                compositeDisposable.add(it)
                progressLiveStatus.postValue(AppConstants.LOADING_STATUS)
            }
            .subscribe({
                progressLiveStatus.postValue(AppConstants.LOADED_STATUS)
                callback.onResult(it,  params.key+pageSize)
            },{
                progressLiveStatus.postValue(AppConstants.ERROR_STATUS)
                setRetry(Action { loadAfter(params, callback) })
            })

    }

    override fun loadBefore(params: LoadParams<Int>, callback: LoadCallback<Int, PhotoModel>) {
        TODO("not implemented") //To change body of created functions use File | Settings | File Templates.
    }

    fun retry() {
        if (retryCompletable != null) {
            compositeDisposable.add(retryCompletable!!
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribe())
        }
    }

    private fun setRetry(action: Action?) {
        retryCompletable = if (action == null) null else Completable.fromAction(action)
    }

}