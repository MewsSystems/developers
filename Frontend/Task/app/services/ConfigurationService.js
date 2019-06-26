import { Routes } from '../constants';

export default {
  fetchConfiguration: () => {
    return fetch(Routes.Configuration).then((res) => {
      if (res.ok) {
        return res.json();
      } else {
        throw 'Cannot fetch configuration';
      }

    });
  }
}