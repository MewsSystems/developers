import { SerializedError } from "@reduxjs/toolkit";

export class LoggerService {
    static logError(error: SerializedError | Error) {
        if (process.env.NODE_ENV === 'production') {
            // Bugsnag.notify(e);
        }
        else {
            console.error(error);
        }
    }
}
