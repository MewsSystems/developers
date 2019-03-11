package sk.cll.mewsapp.data.utils;

import java.util.ArrayList;
import java.util.List;

import androidx.lifecycle.ViewModel;
import sk.cll.mewsapp.data.Photo;

public class MyViewModel extends ViewModel {
    private List<Photo> photos;
    private Photo selected;

    public List<Photo> getPhotos() {
        return photos;
    }

    public void addPhotos(List<Photo> photos) {
        if (this.photos == null) {
            this.photos = new ArrayList<>();
        }
        this.photos.addAll(photos);
    }

    public Photo getSelected() {
        return selected;
    }

    public void setSelected(Photo selected) {
        this.selected = selected;
    }
}
