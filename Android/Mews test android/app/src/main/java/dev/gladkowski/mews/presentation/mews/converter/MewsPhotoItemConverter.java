package dev.gladkowski.mews.presentation.mews.converter;

import java.util.List;

import dev.gladkowski.mews.entity.mews.domain.MewsPhoto;
import dev.gladkowski.mews.entity.mews.presentation.BaseMewsPhotoItem;
import io.reactivex.functions.Function;

/**
 * Converts List<MewsPhoto> object into List<BaseMewsPhotoItem> object
 */
public interface MewsPhotoItemConverter extends Function<List<MewsPhoto>, List<BaseMewsPhotoItem>> {

}
