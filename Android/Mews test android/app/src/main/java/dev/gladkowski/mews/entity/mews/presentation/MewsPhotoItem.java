package dev.gladkowski.mews.entity.mews.presentation;

/**
 * Visual class for Mews Photo
 */
public class MewsPhotoItem extends BaseMewsPhotoItem {

    private Integer albumId;
    private Integer id;
    private String title;
    private String url;
    private String thumbnailUrl;
    private boolean isExpanded;

    public MewsPhotoItem(Integer albumId, Integer id, String title, String url, String thumbnailUrl, boolean isExpanded) {
        this.albumId = albumId;
        this.id = id;
        this.title = title;
        this.url = url;
        this.thumbnailUrl = thumbnailUrl;
        this.isExpanded = isExpanded;
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

    public boolean isExpanded() {
        return isExpanded;
    }

    public void setExpanded(boolean expanded) {
        isExpanded = expanded;
    }
}
