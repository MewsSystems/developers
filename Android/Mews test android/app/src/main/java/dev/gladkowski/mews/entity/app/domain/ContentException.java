package dev.gladkowski.mews.entity.app.domain;

/**
 * Class for content exceptions
 */
public class ContentException extends BaseException {
    public static final String TAG = "ContentException";

    private String message;

    public ContentException(String message) {
        super(TAG);
        this.message = message;
    }

    @Override
    public String getMessage() {
        return message;
    }
}
