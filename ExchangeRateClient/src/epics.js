import { interval, of } from 'rxjs';
import { ajax } from 'rxjs/ajax';
import { map, mergeMap, switchMap, takeUntil, catchError } from 'rxjs/operators';
import { combineEpics, ofType } from 'redux-observable';
import { stringify } from 'qs';
import {
  actionTypes,
  fetchConfigFulfilled,
  fetchConfigFailed,
  fetchRatesFulfilled,
  fetchRatesFailed,
} from './actions';

const fetchConfigEpic = action$ =>
  action$.pipe(
    ofType(actionTypes.FETCH_CONFIG),
    mergeMap(() =>
      ajax.getJSON('http://localhost:3000/configuration').pipe(
        map(response => fetchConfigFulfilled(response)),
        catchError(err => of(fetchConfigFailed(err))),
        takeUntil(action$.pipe(ofType(actionTypes.FETCH_CONFIG_CANCELLED)))
      )
    )
  );

const fetchRatesEpic = action$ =>
  action$.pipe(
    ofType(actionTypes.FETCH_RATES),
    mergeMap(action =>
      interval(action.payload.interval).pipe(
        switchMap(() =>
          ajax
            .getJSON(
              `http://localhost:3000/rates?${stringify(
                { currencyPairIds: action.payload.ids },
                { arrayFormat: 'brackets' }
              )}`
            )
            .pipe(
              map(res => fetchRatesFulfilled(res)),
              catchError(err => of(fetchRatesFailed(action.payload.ids, err)))
            )
        ),
        takeUntil(action$.pipe(ofType(actionTypes.FETCH_RATES_CANCELLED)))
      )
    )
  );

export default combineEpics(fetchConfigEpic, fetchRatesEpic);
