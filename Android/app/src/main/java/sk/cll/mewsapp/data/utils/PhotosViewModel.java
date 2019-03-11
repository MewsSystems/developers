package sk.cll.mewsapp.data.utils;

import java.util.List;

import androidx.lifecycle.LiveData;
import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;
import okhttp3.OkHttpClient;
import retrofit2.Retrofit;
import retrofit2.adapter.rxjava2.RxJava2CallAdapterFactory;
import retrofit2.converter.gson.GsonConverterFactory;
import sk.cll.mewsapp.activities.ItemListActivity;
import sk.cll.mewsapp.data.Photo;
import sk.cll.mewsapp.services.PhotoService;

public class PhotosViewModel extends ViewModel {
    private MutableLiveData<List<Photo>> photos;
    private final MutableLiveData<Photo> selected = new MutableLiveData<>();
    private PhotoService service = new Retrofit.Builder()
            .baseUrl(ItemListActivity.BASE_URL)
            .addConverterFactory(GsonConverterFactory.create())
            .client(new OkHttpClient.Builder().build())
            .addConverterFactory(GsonConverterFactory.create())
            .addCallAdapterFactory(RxJava2CallAdapterFactory.create())
            .build().create(PhotoService.class);

    public LiveData<List<Photo>> getPhotos(int start) {
        if (photos == null) {
            photos = new MutableLiveData<>();
            loadPhotos(start);
        }
        return photos;
    }


    public void select(Photo item) {
        selected.setValue(item);
    }

    public LiveData<Photo> getSelected() {
        return selected;
    }



    private void loadPhotos(int start) {
        photos.setValue(service.getPhotos2(start, 30));
    }

}
