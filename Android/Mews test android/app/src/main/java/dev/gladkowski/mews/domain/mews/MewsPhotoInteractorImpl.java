package dev.gladkowski.mews.domain.mews;

import java.util.List;

import dev.gladkowski.mews.data.repository.mews.MewsPhotoRepository;
import dev.gladkowski.mews.entity.mews.domain.MewsPhoto;
import dev.gladkowski.mews.utils.rx.RxUtils;
import dev.gladkowski.mews.utils.rx.SingleErrorHandler;
import io.reactivex.Single;

/**
 * Implementation of MewsPhotoInteractor
 */
public class MewsPhotoInteractorImpl implements MewsPhotoInteractor {

    private MewsPhotoRepository mewsPhotoRepository;
    private SingleErrorHandler singleErrorHandler;
    private RxUtils rxUtils;

    public MewsPhotoInteractorImpl(MewsPhotoRepository mewsPhotoRepository,
                                   SingleErrorHandler singleErrorHandler,
                                   RxUtils rxUtils) {
        this.mewsPhotoRepository = mewsPhotoRepository;
        this.singleErrorHandler = singleErrorHandler;
        this.rxUtils = rxUtils;
    }

    @Override
    public Single<List<MewsPhoto>> getMewsPhotosByPage(int start, int limit) {
        return mewsPhotoRepository.getMewsPhotosByPage(start, limit)
                .compose((SingleErrorHandler<List<MewsPhoto>>) singleErrorHandler)
                .compose(rxUtils.applySingleSchedulers());    }
}
