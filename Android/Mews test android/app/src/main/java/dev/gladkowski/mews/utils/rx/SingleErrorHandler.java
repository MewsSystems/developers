package dev.gladkowski.mews.utils.rx;

import io.reactivex.Single;
import io.reactivex.SingleSource;
import io.reactivex.SingleTransformer;

public class SingleErrorHandler<T> implements SingleTransformer<T, T> {

    private ErrorProcessing<T> errorProcessing;

    public SingleErrorHandler(ErrorProcessing errorProcessing) {
        this.errorProcessing = errorProcessing;
    }

    @Override
    public SingleSource<T> apply(Single<T> upstream) {
        return upstream.onErrorResumeNext(errorProcessing::getSingleErrors);
    }
}
