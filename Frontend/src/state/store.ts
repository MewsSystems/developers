import { recentMoviesInitialState } from 'state/reducers/movies/recent'
import { rootReducer, initialState, State } from './rootReducer'
import { applyMiddleware, createStore, Store } from 'redux'
import { composeWithDevTools } from 'redux-devtools-extension'
import { createMigrate, persistReducer } from 'redux-persist'
import storage from 'redux-persist/lib/storage'
import thunk from 'redux-thunk'
import {
  MigrationManifest,
  PersistConfig,
  PersistedState,
} from 'redux-persist/es/types'
import createDebounce from 'redux-debounced'

export const STORE_VERSION = 1

const migrations: MigrationManifest = {
  1: (state: PersistedState): any => {
    if (state) {
      return { ...state, recentMovies: recentMoviesInitialState }
    }
  },
}

const persistConfig: PersistConfig<State> = {
  key: 'root',
  version: STORE_VERSION,
  storage,
  whitelist: ['recentMovies'],
  migrate: createMigrate(migrations, {
    debug: process.env.NODE_ENV === 'development',
  }),
}

const configureStore = (): Store<State, any> =>
  createStore(
    persistReducer(persistConfig, rootReducer),
    initialState as any, // TODO: fix conflict between State and _persist types
    composeWithDevTools(applyMiddleware(createDebounce(), thunk))
  )

export const store = configureStore()
