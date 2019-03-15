package com.zeyadassem.apireader.ui.utils

import android.content.Context
import android.net.NetworkInfo
import android.net.ConnectivityManager



object AppConstants {
    const val BASE_URL = "http://jsonplaceholder.typicode.com"
    const val PHOTO_LIST_PATH = "/photos"
    const val INITIAL_LOADING_STATUS = "initial_loading"
    const val LOADING_STATUS = "loading"
    const val LOADED_STATUS = "loaded"
    const val ERROR_STATUS = "error"
    const val PHOTO_MODEL_KEY = "photo_model"

    fun checkInternetConnection(context: Context): Boolean {
        val connectivity = context
            .getSystemService(Context.CONNECTIVITY_SERVICE) as ConnectivityManager
        if (connectivity == null) {
            return false
        } else {
            val info = connectivity.allNetworkInfo
            if (info != null) {
                for (anInfo in info) {
                    if (anInfo.state == NetworkInfo.State.CONNECTED) {
                        return true
                    }
                }
            }
        }
        return false
    }
}