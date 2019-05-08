package dev.gladkowski.mews.utils.rx;

import javax.inject.Inject;

import io.reactivex.Completable;
import io.reactivex.CompletableSource;
import io.reactivex.CompletableTransformer;

public class CompletableErrorHandler implements CompletableTransformer {

    private ErrorProcessing errorProcessing;

    @Inject
    public CompletableErrorHandler(ErrorProcessing errorProcessing) {
        this.errorProcessing = errorProcessing;
    }

    @Override
    public CompletableSource apply(Completable upstream) {
        return upstream.onErrorResumeNext(errorProcessing::getCompletableErrors);
    }
}
