import { AnyAction } from 'redux';

export const isActionAborted = (action: AnyAction) =>
  action.meta.aborted || false;
