package com.mews.task.net

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import com.mews.task.data.NetworkState
import javax.inject.Singleton

@Singleton
class NetworkStateModel {

    private val _state = MutableLiveData<NetworkState>()
    val state: LiveData<NetworkState> = _state

    fun postState(state: NetworkState) {
        _state.postValue(state)
    }
}