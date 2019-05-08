package dev.gladkowski.mews.utils.rx;

import io.reactivex.CompletableTransformer;
import io.reactivex.ObservableTransformer;
import io.reactivex.SingleTransformer;
import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.schedulers.Schedulers;

/**
 * Utils for work with threads
 */
public class RxUtils {

    public <T> CompletableTransformer applyCompleteSchedulers() {
        return observable -> observable.subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread());
    }

    public <T> ObservableTransformer<T, T> applySchedulers() {
        return observable -> observable.subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread());
    }

    public <T> SingleTransformer<T, T> applySingleSchedulers() {
        return observable -> observable.subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread());

    }

    public <T> CompletableTransformer applyCompleteSingleSchedulers() {
        return observable -> observable.subscribeOn(Schedulers.single())
                .observeOn(AndroidSchedulers.mainThread());
    }
}
