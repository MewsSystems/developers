package dev.gladkowski.mews.mews.repository;

import org.junit.Before;
import org.junit.Test;
import org.mockito.Mock;
import org.mockito.MockitoAnnotations;

import java.util.List;

import dev.gladkowski.mews.data.network.MewsPhotoApi;
import dev.gladkowski.mews.data.repository.mews.MewsPhotoRepository;
import dev.gladkowski.mews.data.repository.mews.MewsPhotoRepositoryImpl;
import dev.gladkowski.mews.data.repository.mews.converter.MewsPhotoResponseConverter;
import dev.gladkowski.mews.entity.mews.data.MewsPhotoResponse;
import dev.gladkowski.mews.entity.mews.domain.MewsPhoto;
import io.reactivex.Single;
import io.reactivex.observers.TestObserver;

import static org.mockito.Matchers.anyInt;
import static org.mockito.Matchers.anyListOf;
import static org.mockito.Mockito.times;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

public class MewsPhotoRepositoryTest {

    private MewsPhotoRepository mewsPhotoRepository;

    @Mock
    private MewsPhotoApi mewsPhotoApi;
    @Mock
    private MewsPhotoResponseConverter converter;
    @Mock
    private List<MewsPhotoResponse> mockedResponseList;
    @Mock
    private List<MewsPhoto> mockedPhotoList;

    @Before
    public void setup() {
        MockitoAnnotations.initMocks(this);
        mewsPhotoRepository = new MewsPhotoRepositoryImpl(mewsPhotoApi, converter);
    }

    @Test
    public void getMewsPhotosByPageShouldSuccess() {

        when(mewsPhotoApi.getMewsPhotosByPage(anyInt(), anyInt())).thenReturn(Single.just(mockedResponseList));
        try {
            when(converter.apply(anyListOf(MewsPhotoResponse.class))).thenReturn(mockedPhotoList);
        } catch (Exception e) {
            e.printStackTrace();
        }

        TestObserver testObserver = mewsPhotoRepository.getMewsPhotosByPage(anyInt(), anyInt()).test();
        testObserver.assertNoErrors();
        testObserver.awaitTerminalEvent();

        verify(mewsPhotoApi, times(1)).getMewsPhotosByPage(anyInt(), anyInt());
    }
}
