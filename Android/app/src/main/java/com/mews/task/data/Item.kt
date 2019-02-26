package com.mews.task.data

import android.os.Parcelable
import com.google.gson.annotations.SerializedName
import kotlinx.android.parcel.Parcelize

data class ItemDTO(

    @SerializedName("id")
    val id: Long,

    @SerializedName("title")
    val title: String,

    @SerializedName("thumbnailUrl")
    val thumbUrl: String,

    @SerializedName("url")
    val url: String
) {
    fun toDomain(): Item {
        return Item(
            id = id,
            title = title,
            thumbUrl = thumbUrl,
            url = url
        )
    }
}

@Parcelize
data class Item(
    val id: Long,
    val title: String,
    val thumbUrl: String,
    val url: String
) : Parcelable