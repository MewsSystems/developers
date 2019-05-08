package dev.gladkowski.mews.presentation.mews.converter;

import java.util.ArrayList;
import java.util.List;

import dev.gladkowski.mews.entity.mews.domain.MewsPhoto;
import dev.gladkowski.mews.entity.mews.presentation.BaseMewsPhotoItem;
import dev.gladkowski.mews.entity.mews.presentation.MewsPhotoItem;

/**
 * Implementation of MewsPhotoItemConverter
 */
public class MewsPhotoItemConverterImpl implements MewsPhotoItemConverter {

    @Override
    public List<BaseMewsPhotoItem> apply(List<MewsPhoto> mewsPhotos) throws Exception {
        List<BaseMewsPhotoItem> viewModelList = new ArrayList<>();

        for (MewsPhoto mewsPhoto : mewsPhotos) {
            viewModelList.add(new MewsPhotoItem(
                    mewsPhoto.getAlbumId(),
                    mewsPhoto.getId(),
                    mewsPhoto.getTitle(),
                    mewsPhoto.getUrl(),
                    mewsPhoto.getThumbnailUrl(),
                    false // all views are displayed not expanded by default
            ));
        }
        return viewModelList;
    }
}
