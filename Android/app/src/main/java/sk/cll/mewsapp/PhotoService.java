package sk.cll.mewsapp;

import java.util.List;

import io.reactivex.Observable;
import retrofit2.http.GET;
import retrofit2.http.Query;

public interface PhotoService {

    @GET("photos")
    Observable<List<Photo>> getPhotos(@Query("_start") int start, @Query("_limit") int limit);
}
