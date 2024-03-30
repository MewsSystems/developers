// NB!
// Using this outside of iocContainer since we also want log issues with iocContainer itself
export class Logger {
    error(message?: any, ...optionalParams: any[]): void {
        // todo: send to kibana, sentry, etc.
        console.error(message, ...optionalParams);
    }
}

export const logger = new Logger();