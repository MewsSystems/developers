package dev.gladkowski.mews.utils.rx;

public interface ErrorResourceProvider {

    String getUnknownHostException();

    String getSocketTimeoutException();

    String getConnectionErrorException();

    String getJsonSyntaxException();

    String getUnknownException();

    String getGenericException();

    String getInternalServerException();

}
