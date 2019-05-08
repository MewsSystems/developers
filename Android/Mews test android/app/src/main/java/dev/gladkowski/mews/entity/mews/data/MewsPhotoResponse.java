package dev.gladkowski.mews.entity.mews.data;

import com.google.gson.annotations.SerializedName;

/**
 * Response class for MewsPhoto
 */
public class MewsPhotoResponse {

    @SerializedName("albumId")
    private Integer albumId;

    @SerializedName("id")
    private Integer id;

    @SerializedName("title")
    private String title;

    @SerializedName("url")
    private String url;

    @SerializedName("thumbnailUrl")
    private String thumbnailUrl;

    public Integer getAlbumId() {
        return albumId;
    }

    public Integer getId() {
        return id;
    }

    public String getTitle() {
        return title;
    }

    public String getUrl() {
        return url;
    }

    public String getThumbnailUrl() {
        return thumbnailUrl;
    }
}