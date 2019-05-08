package dev.gladkowski.mews.domain.mews;

import java.util.List;

import dev.gladkowski.mews.entity.mews.domain.MewsPhoto;
import io.reactivex.Single;

/**
 * Interactor for Mews Photos
 */
public interface MewsPhotoInteractor {

    /**
     * Get list of mews photos by page
     *
     * @param start items already loaded
     * @param limit number of items per page
     */
    Single<List<MewsPhoto>> getMewsPhotosByPage(int start, int limit);
}
