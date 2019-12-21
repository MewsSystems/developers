import * as types from "./types";
import { getRatesDataAction } from "./index";

export const changeFilterAction = filter => ({
  type: types.CHANGE_FILTER,
  filter
});

//Handler for defaul filterConfig, (allow all)
export const createDefaultFilter = () => {
  return async (dispatch, getState) => {
    const config = await getState().config;
    let defaultFilter = {};
    Object.keys(config).forEach(configId => {
      defaultFilter[configId] = true;
    });
    await dispatch(changeFilterAction(defaultFilter));
  };
};

//Handler for change filter, worked by 2 types (1-select by one rate, 2-select several rates by dropDownFilter)
export const changeFilter = (key, type) => {
  return async (dispatch, getState) => {
    let filter = await Object.assign({}, getState().filter);

    switch (type) {
      case "select":
        if (Object.keys(filter).length === 1 && filter[key] === true) {
          await dispatch(createDefaultFilter());
          await dispatch(getRatesDataAction());
        } else {
          filter = {};
          filter[key] = true;
          await dispatch(changeFilterAction(filter));
          await dispatch(getRatesDataAction());
        }
        break;

      case "filter":
        if (filter[key]) {
          delete filter[key];
        } else {
          filter[key] = true;
        }
        await dispatch(changeFilterAction(filter));
        await dispatch(getRatesDataAction());
        break;
      default:
        await dispatch(createDefaultFilter());
        await dispatch(getRatesDataAction());
        break;
    }
  };
};
