package com.marekfeifrlik.android.mewsphotos.data

import com.google.gson.annotations.SerializedName

data class PhotoEntity (
    @SerializedName("albumId") val albumId: Int = 0,
    @SerializedName("id") val id: Int = 0,
    @SerializedName("title") val title: String = "",
    @SerializedName("url") val url: String = "",
    @SerializedName("thumbnailUrl") val thumbnailUrl: String = ""
)