package dev.gladkowski.mews.utils.date;

import com.google.gson.JsonDeserializer;
import com.google.gson.JsonSerializer;

import org.joda.time.DateTime;

/**
 * Time serialization
 */
public interface DateTimeResponseConverter extends JsonSerializer<DateTime>, JsonDeserializer<DateTime> {
}
