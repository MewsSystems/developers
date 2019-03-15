package com.zeyadassem.apireader.network.models

import android.os.Parcelable
import kotlinx.android.parcel.Parcelize

@Parcelize
data class PhotoModel(val albumId: Int, val id: Int, val title: String, val url: String, val thumbnailUrl: String) :
    Parcelable