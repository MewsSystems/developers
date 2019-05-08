package dev.gladkowski.mews.data.network;

import java.util.List;

import dev.gladkowski.mews.entity.mews.data.MewsPhotoResponse;
import io.reactivex.Single;
import retrofit2.http.GET;
import retrofit2.http.Query;

public interface MewsPhotoApi {

    /**
     * Get list of Mews photos by page
     *
     * @param start items already loaded
     * @param limit number of items per page
     */
    @GET("/photos")
    Single<List<MewsPhotoResponse>> getMewsPhotosByPage(@Query("_start") int start, @Query("_limit") int limit);
}
