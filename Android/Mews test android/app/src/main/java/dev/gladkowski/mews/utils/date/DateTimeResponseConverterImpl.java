package dev.gladkowski.mews.utils.date;

import com.google.gson.JsonDeserializationContext;
import com.google.gson.JsonElement;
import com.google.gson.JsonParseException;
import com.google.gson.JsonPrimitive;
import com.google.gson.JsonSerializationContext;

import org.joda.time.DateTime;
import org.joda.time.DateTimeZone;

import java.lang.reflect.Type;
import java.util.Date;

/**
 * Implementation of DateTimeResponseConverter
 */
public class DateTimeResponseConverterImpl implements DateTimeResponseConverter {
    @Override
    public DateTime deserialize(JsonElement json, Type typeOfT, JsonDeserializationContext context) throws JsonParseException {
        try {
            return new DateTime(json.getAsString(), DateTimeZone.UTC).withZone(DateTimeZone.getDefault());
        } catch (IllegalArgumentException e) {
            Date date = context.deserialize(json, Date.class);
            return new DateTime(date);
        }
    }

    @Override
    public JsonElement serialize(DateTime src, Type typeOfSrc, JsonSerializationContext context) {
        return new JsonPrimitive(src.withZone(DateTimeZone.UTC).toLocalDateTime().toString());
    }
}
