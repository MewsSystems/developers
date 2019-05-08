package dev.gladkowski.mews.entity.mews.domain;

/**
 * Domain class for MewsPhoto
 */
public class MewsPhoto {

    private Integer albumId;
    private Integer id;
    private String title;
    private String url;
    private String thumbnailUrl;

    public MewsPhoto(Integer albumId, Integer id, String title, String url, String thumbnailUrl) {
        this.albumId = albumId;
        this.id = id;
        this.title = title;
        this.url = url;
        this.thumbnailUrl = thumbnailUrl;
    }

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
