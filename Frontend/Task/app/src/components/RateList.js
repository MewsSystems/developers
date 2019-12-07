import React, { useEffect } from "react";
import { useSelector, useDispatch } from "react-redux";
import { Spinner, Alert } from "reactstrap";
import { firstLoad, fetchRates } from "../store/actions";

export const RateList = () => {
  const dispatch = useDispatch();

  useEffect(() => {
    dispatch(firstLoad());
  }, [dispatch]);

  useEffect(() => {
    const interval = setInterval(() => {
      dispatch(fetchRates());
    }, 15000);
    return () => clearInterval(interval);
  }, [dispatch]);

  const { currencyPairs, filteredPairs, loading, error } = useSelector(
    state => state
  );

  return loading ? (
    <Spinner type="grow" color="success" />
  ) : error && filteredPairs.length > 0 ? (
    <Alert color="danger" fade={false}>
      {error}
    </Alert>
  ) : (
    <>
      {Object.entries(currencyPairs)
        .filter(([id]) => filteredPairs.includes(id))
        .map(([, { shortcut, value, trend }]) => (
          <div key={shortcut}>
            {shortcut}: {value} ({trend})
          </div>
        ))}
    </>
  );
};
