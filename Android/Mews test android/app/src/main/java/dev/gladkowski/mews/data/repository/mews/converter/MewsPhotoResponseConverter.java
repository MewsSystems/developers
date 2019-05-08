package dev.gladkowski.mews.data.repository.mews.converter;

import java.util.List;

import dev.gladkowski.mews.entity.mews.data.MewsPhotoResponse;
import dev.gladkowski.mews.entity.mews.domain.MewsPhoto;
import io.reactivex.functions.Function;

/**
 * Converts List<MewsPhotoResponse> object into List<MewsPhoto> object
 */
public interface MewsPhotoResponseConverter extends Function<List<MewsPhotoResponse>, List<MewsPhoto>> {

}
