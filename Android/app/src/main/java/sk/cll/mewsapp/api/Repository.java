package sk.cll.mewsapp.api;

import java.util.List;

import io.reactivex.Observable;
import sk.cll.mewsapp.activities.ItemListActivity;
import sk.cll.mewsapp.data.Photo;

public class Repository {
    private PhotoService apiCallInterface;

    public Repository(PhotoService apiCallInterface) {
        this.apiCallInterface = apiCallInterface;
    }

    public Observable<List<Photo>> executePhotoApi(int index) {
        return    apiCallInterface.getPhotos(index, ItemListActivity.LIMIT);
    }
}
