import { addIconsToLibrary } from 'constants/icons'
import '@testing-library/jest-dom/extend-expect'
import { configure } from 'enzyme'
import Adapter from 'enzyme-adapter-react-16'
import axios from 'axios'

window._envConfig = {
  GA_TRACKING_CODE: '',
  MDB_BASE_URL: 'https://api.themoviedb.org/3',
  MDB_API_KEY: '03b8572954325680265531140190fd2a',
}

configure({ adapter: new Adapter() })
addIconsToLibrary()

axios.defaults.baseURL = window._envConfig.MDB_BASE_URL
