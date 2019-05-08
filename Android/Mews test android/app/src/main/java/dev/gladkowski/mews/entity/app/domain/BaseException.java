package dev.gladkowski.mews.entity.app.domain;

/**
 * Basic class for exceptions
 */
public abstract class BaseException extends Exception {
    private String tag;

    public BaseException(String tag) {
        this.tag = tag;
    }

    public String getTag() {
        return tag;
    }
}
