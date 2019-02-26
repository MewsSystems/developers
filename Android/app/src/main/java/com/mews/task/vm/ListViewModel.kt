package com.mews.task.vm

import android.annotation.SuppressLint
import androidx.annotation.RestrictTo
import androidx.lifecycle.ViewModel
import androidx.paging.LivePagedListBuilder
import androidx.paging.PagedList
import com.mews.task.net.ItemDataSourceFactory
import com.mews.task.net.NetworkStateModel
import toothpick.Scope

class ListViewModel @RestrictTo(RestrictTo.Scope.TESTS) constructor(
    itemFactory: ItemDataSourceFactory,
    pagingConfig: PagedList.Config,
    networkStateModel: NetworkStateModel
) : ViewModel() {

    val items = LivePagedListBuilder(itemFactory, pagingConfig).build()
    val networkState = networkStateModel.state

    fun reload() {
        items.value?.dataSource?.invalidate()
    }

    companion object {
        @SuppressLint("RestrictedApi")
        fun create(scope: Scope): ListViewModel {
            return ListViewModel(
                itemFactory = scope.getDependency(),
                pagingConfig = scope.getDependency(),
                networkStateModel = scope.getDependency()
            )
        }
    }
}
