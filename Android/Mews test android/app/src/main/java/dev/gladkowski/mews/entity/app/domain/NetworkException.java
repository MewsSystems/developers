package dev.gladkowski.mews.entity.app.domain;

/**
 * Class for network exceptions
 */
public class NetworkException extends BaseException {
    public static final String TAG = "NetworkException";

    private String message;

    public NetworkException(String message) {
        super(TAG);
        this.message = message;
    }

    @Override
    public String getMessage() {
        return message;
    }
}
