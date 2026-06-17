import LocalStoreUtil from '../utils/LocalStorageUtil';
import useResetLocalStorageOnRefresh from './useResetLocalStorageOnRefresh';

const useLocalStorage = () => {
    // Reset local storage on refresh so we can start with empty form data
    useResetLocalStorageOnRefresh();

    const fromMovieDetailsPage = LocalStoreUtil.get('fromMovieDetailsPage');
    const localSearchQuery = LocalStoreUtil.get('searchQuery');
    const localPageNumber = LocalStoreUtil.get('pageNumber');

    return { fromMovieDetailsPage, localSearchQuery, localPageNumber };
};

export default useLocalStorage;
