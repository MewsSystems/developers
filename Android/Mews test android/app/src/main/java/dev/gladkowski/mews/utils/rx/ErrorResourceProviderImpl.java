package dev.gladkowski.mews.utils.rx;

import android.content.Context;

import javax.inject.Inject;

import dev.gladkowski.mews.R;


public class ErrorResourceProviderImpl implements ErrorResourceProvider {

    private Context context;

    @Inject
    public ErrorResourceProviderImpl(Context context) {
        this.context = context;
    }

    @Override
    public String getUnknownHostException() {
        return context.getString(R.string.error_no_internet);
    }

    @Override
    public String getSocketTimeoutException() {
        return context.getString(R.string.error_socket_timeout);
    }

    @Override
    public String getConnectionErrorException() {
        return context.getString(R.string.error_no_internet);
    }

    @Override
    public String getJsonSyntaxException() {
        return context.getString(R.string.error_json);
    }

    @Override
    public String getUnknownException() {
        return context.getString(R.string.error_unknown);
    }

    @Override
    public String getGenericException() {
        return context.getString(R.string.error_generic);
    }

    @Override
    public String getInternalServerException() {
        return context.getString(R.string.error_internal_server);
    }
}
