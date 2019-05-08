package dev.gladkowski.mews.presentation.mews.provider;

import android.content.Context;

import dev.gladkowski.mews.R;


/**
 * Implementation of MewsResourceProvider
 */
public class MewsResourceProviderImpl implements MewsResourceProvider {

    private Context context;

    public MewsResourceProviderImpl(Context context) {
        this.context = context;
    }

    @Override
    public String getTitle() {
        return context.getString(R.string.title_mews_photos);
    }

}
