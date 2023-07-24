/**
 * @description Scheduler class to ensure the callout of ExchangeRatesService is running once a day
 * the ExchangeRatesService execution will run in a separate execution scope with @Future
 */
global class ExchangeRatesScheduler implements Schedulable {
    global void execute(SchedulableContext sc) {
        ExchangeRatesService.getRates(System.now());
    }
}