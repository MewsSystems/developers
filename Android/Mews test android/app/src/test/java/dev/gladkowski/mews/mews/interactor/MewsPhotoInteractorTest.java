package dev.gladkowski.mews.mews.interactor;

import org.junit.Before;
import org.junit.ClassRule;
import org.junit.Test;
import org.mockito.Mock;
import org.mockito.MockitoAnnotations;

import java.util.List;

import dev.gladkowski.mews.data.repository.mews.MewsPhotoRepository;
import dev.gladkowski.mews.domain.mews.MewsPhotoInteractor;
import dev.gladkowski.mews.domain.mews.MewsPhotoInteractorImpl;
import dev.gladkowski.mews.entity.mews.domain.MewsPhoto;
import dev.gladkowski.mews.rxutils.RxImmediateSchedulerRule;
import dev.gladkowski.mews.utils.rx.ErrorProcessing;
import dev.gladkowski.mews.utils.rx.ErrorResourceProvider;
import dev.gladkowski.mews.utils.rx.RxUtils;
import dev.gladkowski.mews.utils.rx.SingleErrorHandler;
import io.reactivex.Single;
import io.reactivex.observers.TestObserver;

import static org.mockito.Matchers.anyInt;
import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.times;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

public class MewsPhotoInteractorTest {
    private MewsPhotoInteractor interactor;

    @ClassRule
    public static final RxImmediateSchedulerRule schedulers = new RxImmediateSchedulerRule();

    @Mock
    private MewsPhotoRepository repository;
    @Mock
    private SingleErrorHandler singleErrorHandler;
    @Mock
    private List<MewsPhoto> mockArrayList;


    @Before
    public void setup() {
        MockitoAnnotations.initMocks(this);
        RxUtils rxUtils = new RxUtils();
        ErrorResourceProvider mockErrorRes = mock(ErrorResourceProvider.class);
        ErrorProcessing errorProcessing = new ErrorProcessing(mockErrorRes);
        singleErrorHandler = new SingleErrorHandler(errorProcessing);
        interactor = new MewsPhotoInteractorImpl(repository, singleErrorHandler, rxUtils);
    }

    @Test
    public void getMewsPhotosByPageShouldSuccess() {

        when(repository.getMewsPhotosByPage(anyInt(), anyInt())).thenReturn(Single.just(mockArrayList));

        TestObserver testObserver = interactor.getMewsPhotosByPage(anyInt(), anyInt()).test();
        testObserver.awaitTerminalEvent();
        testObserver.assertNoErrors();

        verify(repository, times(1)).getMewsPhotosByPage(anyInt(), anyInt());
    }
}
