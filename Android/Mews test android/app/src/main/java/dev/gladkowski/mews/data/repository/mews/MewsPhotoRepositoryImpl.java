package dev.gladkowski.mews.data.repository.mews;


import java.util.List;

import dev.gladkowski.mews.data.network.MewsPhotoApi;
import dev.gladkowski.mews.data.repository.mews.converter.MewsPhotoResponseConverter;
import dev.gladkowski.mews.entity.mews.domain.MewsPhoto;
import io.reactivex.Single;

/**
 * Implementation of MewsPhotoRepository
 */
public class MewsPhotoRepositoryImpl implements MewsPhotoRepository {

    private MewsPhotoApi mewsPhotoApi;
    private MewsPhotoResponseConverter converter;

    public MewsPhotoRepositoryImpl(MewsPhotoApi mewsPhotoApi,
                                   MewsPhotoResponseConverter converter) {
        this.mewsPhotoApi = mewsPhotoApi;
        this.converter = converter;
    }

    /**
     * Get list of mews photos by page
     *
     * @param start items already loaded
     * @param limit number of items per page
     */
    @Override
    public Single<List<MewsPhoto>> getMewsPhotosByPage(int start, int limit) {
        return mewsPhotoApi.getMewsPhotosByPage(start, limit)
                .map(converter);
    }
}
