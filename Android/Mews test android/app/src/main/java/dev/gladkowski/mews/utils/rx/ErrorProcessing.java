package dev.gladkowski.mews.utils.rx;

import android.support.annotation.NonNull;

import com.google.gson.JsonSyntaxException;

import java.net.ConnectException;
import java.net.SocketTimeoutException;
import java.net.UnknownHostException;
import java.util.NoSuchElementException;

import dev.gladkowski.mews.entity.app.domain.BaseException;
import dev.gladkowski.mews.entity.app.domain.ContentException;
import dev.gladkowski.mews.entity.app.domain.NetworkException;
import dev.gladkowski.mews.entity.app.domain.ServiceCodeException;
import dev.gladkowski.mews.entity.app.domain.TitleException;
import io.reactivex.Completable;
import io.reactivex.Observable;
import io.reactivex.Single;
import retrofit2.HttpException;

public class ErrorProcessing<T> {

    private ErrorResourceProvider resourceProvider;

    public ErrorProcessing(ErrorResourceProvider resourceProvider) {
        this.resourceProvider = resourceProvider;
    }

    Observable<T> geObservableErrors(Throwable throwable) {
        return Observable.error(() -> throwProcessException(throwable));
    }

    Single<T> getSingleErrors(Throwable throwable) {
        return Single.error(() -> throwProcessException(throwable));
    }

    Completable getCompletableErrors(Throwable throwable) {
        return Completable.error(() -> throwProcessException(throwable));
    }

    @NonNull
    private Throwable throwProcessException(Throwable throwable) throws TitleException,
            NetworkException, ServiceCodeException, ContentException {

        if (throwable instanceof UnknownHostException) {
            throw new NetworkException(resourceProvider.getUnknownHostException());
        }

        if (throwable instanceof ConnectException) {
            throw new NetworkException(resourceProvider.getConnectionErrorException());
        }

        if (throwable instanceof SocketTimeoutException) {
            throw new NetworkException(resourceProvider.getSocketTimeoutException());
        }

        if (throwable instanceof JsonSyntaxException) {
            throw new TitleException("", resourceProvider.getJsonSyntaxException());
        }

        if (throwable instanceof HttpException) {
            throw new ServiceCodeException(((HttpException) throwable).code());
        }

        if (throwable instanceof NoSuchElementException) {
            throw new ContentException("");
        }

        if (throwable instanceof BaseException) {
            return throwable;
        } else {
            return new TitleException("", throwable.getMessage());
        }
    }

}
