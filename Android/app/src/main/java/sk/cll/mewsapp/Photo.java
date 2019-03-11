package sk.cll.mewsapp;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class Photo {

    @SerializedName("title")
    @Expose
    private String title;

    @SerializedName("id")
    @Expose
    private int id;

    @SerializedName("albumId")
    @Expose
    private int albumId;

    @SerializedName("url")
    @Expose
    private String url;

    @SerializedName("thumbnailUrl")
    @Expose
    private String thumbnailUrl;

    private boolean isExpanded;

    public int getId() {
        return id;
    }


    public int getAlbumId() {
        return albumId;
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

    public String getTitle() {
        return title;
    }

}
