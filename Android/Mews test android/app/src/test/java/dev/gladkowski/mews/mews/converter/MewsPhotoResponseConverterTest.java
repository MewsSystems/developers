package dev.gladkowski.mews.mews.converter;

import org.junit.Test;

import java.util.ArrayList;
import java.util.List;

import dev.gladkowski.mews.data.repository.mews.converter.MewsPhotoResponseConverter;
import dev.gladkowski.mews.data.repository.mews.converter.MewsPhotoResponseConverterImpl;
import dev.gladkowski.mews.entity.mews.data.MewsPhotoResponse;
import dev.gladkowski.mews.entity.mews.domain.MewsPhoto;

import static org.junit.Assert.assertEquals;
import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.when;

public class MewsPhotoResponseConverterTest {

    @Test
    public void applyShouldSuccess() {

        List<MewsPhotoResponse> responseList = new ArrayList<>();
        MewsPhotoResponse mewsResponse = mock(MewsPhotoResponse.class);
        when(mewsResponse.getId()).thenReturn(10);
        when(mewsResponse.getAlbumId()).thenReturn(11);
        when(mewsResponse.getThumbnailUrl()).thenReturn("thumbnailUrl");
        when(mewsResponse.getUrl()).thenReturn("url");
        when(mewsResponse.getTitle()).thenReturn("title");
        responseList.add(mewsResponse);


        List<MewsPhoto> mewsPhotos = null;
        MewsPhotoResponseConverter converter = new MewsPhotoResponseConverterImpl();
        try {
            mewsPhotos = converter.apply(responseList);
        } catch (Exception e) {
            e.printStackTrace();
        }

        if (mewsPhotos != null) {
            MewsPhoto item = mewsPhotos.get(0);

            assertEquals(new Integer(10), item.getId());
            assertEquals(new Integer(11), item.getAlbumId());
            assertEquals("thumbnailUrl", item.getThumbnailUrl());
            assertEquals("url", item.getUrl());
            assertEquals("title", item.getTitle());
        } else {
            assertEquals(0, 1); //test fails
        }
    }
}
