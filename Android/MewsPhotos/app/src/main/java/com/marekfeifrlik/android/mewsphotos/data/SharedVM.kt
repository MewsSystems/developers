package com.marekfeifrlik.android.mewsphotos.data

import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import com.marekfeifrlik.android.mewsphotos.data.PhotoEntity

class SharedViewModel : ViewModel() {
    val selected = MutableLiveData<PhotoEntity>()
    val isOrientationLand: MutableLiveData<Boolean> = MutableLiveData()

    fun select(item: PhotoEntity) {
        selected.value = item
    }

    fun setOrientationLand(orientation: Boolean) {
        isOrientationLand.value = orientation
    }
}