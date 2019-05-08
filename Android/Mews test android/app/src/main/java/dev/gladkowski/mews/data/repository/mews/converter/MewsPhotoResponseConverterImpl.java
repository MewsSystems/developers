package dev.gladkowski.mews.data.repository.mews.converter;

import java.util.ArrayList;
import java.util.List;

import dev.gladkowski.mews.entity.mews.data.MewsPhotoResponse;
import dev.gladkowski.mews.entity.mews.domain.MewsPhoto;

public class MewsPhotoResponseConverterImpl implements MewsPhotoResponseConverter {

    @Override
    public List<MewsPhoto> apply(List<MewsPhotoResponse> responseList) throws Exception {
        List<MewsPhoto> result = new ArrayList<>();

        for (MewsPhotoResponse response : responseList) {
            result.add(new MewsPhoto(
                    response.getAlbumId(),
                    response.getId(),
                    response.getTitle(),
                    response.getUrl(),
                    response.getThumbnailUrl()
            ));
        }
        return result;
    }
}
